using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Sockets;
using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Application.Events;
using TicketManagementSystem.Application.Events.TicketEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Enums;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Application.Services;

public class TicketService(
    ITicketRepository ticketRepository,
    IMapper mapper,
    IServiceProvider serviceProvider,
    IEventPublisher eventPublisher,
    IAppLogger logger,
    IUserService userService,
    ITicketAssignmentService ticketAssignmentService)
    : ITicketService
{
    private readonly IAppLogger _logger = logger;
    private readonly IUserService _userService = userService;


    public async Task<IEnumerable<Ticket>> GetAllTicketsAsync(CancellationToken ct)
    {
        await using var logger = serviceProvider.GetRequiredService<IAppLogger>();
        try
        {
            ct.ThrowIfCancellationRequested();
            
            var tickets = await ticketRepository.GetAllTickets(CancellationToken.None);
            if (!tickets.IsError)
            {
                return tickets.Value;
            }
            await logger.LogWarning("No tickets found.", nameof(TicketService));
        }

        catch (Exception e)
        {
            await logger.LogError(e.Message, e, "Database", e.StackTrace!);
        }
        
        return [];
    }
    public async Task AssignTicketToUserAsync(int ticketId, string userId, CancellationToken ct)
    {
        await using var logger = serviceProvider.GetRequiredService<IAppLogger>();
        try
        {
            // Implementation for assigning ticket to user goes here.
            var ticket = await ticketRepository.GetTicketById(ticketId, ct);

            if (ticket.IsError)
            {
                await logger.LogWarning($"Ticket with ID {ticketId} not found.", nameof(TicketService));
                return;
            }
            ticket.Value.AssignedTo = userId;
            ticket.Value.UpdatedAt = DateTime.Now;
            await ticketRepository.UpdateTicket(ticket.Value);
            await  logger.LogInfo($"Ticket {ticketId} assigned to user {userId} at {DateTime.Now}", nameof(TicketService));
        }
        catch (Exception ex)
        {
            if (ex.StackTrace != null)
                await logger.LogError("Error assigning ticket to user", ex, nameof(TicketService), ex.StackTrace);
        }
    }

    public async Task<TicketDto> CreateTicketAsync(TicketDto dto, CancellationToken ct)
    {
        await using var logger = serviceProvider.GetRequiredService<IAppLogger>();
        await using var userService = serviceProvider.GetRequiredService<IUserService>();
        try
        {
            // Implementation for creating a ticket goes here.
            ct.ThrowIfCancellationRequested();

            var ticket = mapper.Map<Ticket>(dto);
            ticket.Status = TicketStatus.Open;
            ticket.Priority = TicketPriority.Medium;
            ticket.UpdatedAt = ticket.CreatedAt;
            ticket.AssignedTo = ticketAssignmentService.GetAssigneeForCategory(dto.Category);


            var user = await userService.GetCurrentUser();
            ticket.Author = $"{user.Vorname} {user.Name}";
            ticket.User = user;
            ticket.UserId = user.Id;
            await ticketRepository.AddTicket(ticket);

            var ticketCreatedEvent = new TicketCreatedEvent(ticket.Id, ticket.Title, ticket.Author, ticket.AssignedTo!);
        
            await eventPublisher.PublishEventAsync(ticketCreatedEvent, ct);
            return dto;
        }
        catch (Exception ex)
        {
            if (ex.StackTrace != null)
            {
                await logger.LogError("Error creating ticket", ex, nameof(TicketService), ex.StackTrace);
            }
            return null!;
        }
    }

    public async Task<Ticket?> UpdateTicketAsync(int ticketId, TicketUpdateDto dto, CancellationToken ct)
    {
        await using var logger = serviceProvider.GetRequiredService<IAppLogger>();
        await using var userService = serviceProvider.GetRequiredService<IUserService>();
        
        var changes = new Dictionary<string, (object? oldValue, object? newValue)>();
       

        var currentUser = await userService.GetCurrentUser();

        

        var result = await ticketRepository.GetTicketById(ticketId, ct);
        if (result.IsError || result.Value == null)
        {
            throw new Exception($"Ticket with Id {ticketId} not found");
        }

        var currentTicket = result.Value;

        if (dto.Priority != currentTicket.Priority)
        {
            ct.ThrowIfCancellationRequested();
            changes[nameof(currentTicket.Priority)] = (currentTicket.Priority, dto.Priority);
            await eventPublisher.PublishEventAsync(
                new TicketPriorityChangedEvent(currentTicket, currentTicket.Priority, dto.Priority, currentUser), ct);
            
            currentTicket.Priority = dto.Priority;
        }

        if (dto.Status != currentTicket.Status)
        {
            ct.ThrowIfCancellationRequested();
            changes[nameof(currentTicket.Status)] = (currentTicket.Status, dto.Status);
            await eventPublisher.PublishEventAsync(
                new TicketStatusChangedEvent(currentTicket, currentTicket.Status, dto.Status, currentTicket.AssignedTo!, currentUser), ct);
            
            currentTicket.Status = dto.Status;
        }

        if ( dto.AssignedTo != null && dto.AssignedTo != currentTicket.AssignedTo)
        {
            ct.ThrowIfCancellationRequested();
            changes[nameof(currentTicket.AssignedTo)] = (currentTicket.AssignedTo, dto.Status);
            await eventPublisher.PublishEventAsync(
                new TicketOwnerChangedEvent(currentTicket, currentTicket.AssignedTo!, dto.AssignedTo, currentUser.Name), ct);
            
            currentTicket.AssignedTo = dto.AssignedTo;
        }

        if (dto.NewComments != null)
        {
            ct.ThrowIfCancellationRequested();

            changes[nameof(currentTicket.Comments)] = (currentTicket.Comments, dto.NewComments);

            foreach (var commentDto in dto.NewComments)
            {
                var comment = new TicketComment
                    { Text = commentDto.Text, 
                        CreatedAt = DateTime.Now,
                        Author = commentDto.Author,
                        User = currentUser,
                        UserId = currentUser.Id
                    };
                currentTicket.Comments.Add(comment);
                
                await eventPublisher.PublishEventAsync(
                    new CommentAddedToTicketEvent(ticketId, comment.Text!, currentUser.Name), ct);
            }

            changes[nameof(currentTicket.Attachments)] = (currentTicket.Comments.Count - dto.NewComments.Count,
                currentTicket.Comments.Count);
        }

        if (dto.NewAttachments != null)
        {
            var oldCount =currentTicket.Attachments.Count;
            foreach (var attachmentDto in dto.NewAttachments)
            {
                ct.ThrowIfCancellationRequested();
                
                var attachment = new TicketAttachment
                {
                    FileName = attachmentDto.FileName, 
                    Url = attachmentDto.Url, 
                    Data = attachmentDto.Data,
                    User = currentUser,
                    UserId = currentUser.Id
                };

                currentTicket.Attachments.Add(attachment);

                await eventPublisher.PublishEventAsync(
                    new AttachmentAddedToTicketEvent(ticketId, attachment.FileName, currentUser.Name), ct);
            }

            changes[nameof(currentTicket.Attachments)] = (currentTicket.Attachments.Count - dto.NewAttachments.Count,
                currentTicket.Attachments.Count);
          
        }

        if (changes.Count == 0)
        {
            return currentTicket;
        }

        await ticketRepository.UpdateTicket(currentTicket);

        await eventPublisher.PublishEventAsync(new TicketUpdatedEvent(ticketId, currentUser.Name, changes), ct);
        return currentTicket;
    }

    public async Task<Ticket?> GetTicketById(int ticketId, CancellationToken ct)
    {
        await using var logger = serviceProvider.GetRequiredService<IAppLogger>();
        try
        {
            var existingTicket = await ticketRepository.GetTicketById(ticketId, ct);
            if (!existingTicket.IsError)
            {
                return existingTicket.Value;
            }
        }
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, "Database", e.StackTrace!);
        }

        return null;
    }

    public async Task<IEnumerable<TicketAttachment>> GetAttachmentsByTicketId(int ticketId, CancellationToken ct)
    {
        await using var logger = serviceProvider.GetRequiredService<IAppLogger>();
        try
        {
            var result = await ticketRepository.GetAttachmentsByTicketId(ticketId, ct);
            if (!result.IsError)
            {
                return result.Value;
            }
        }
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, "Database", e.StackTrace!);
        }
        return [];
    }

    public async Task<IEnumerable<TicketComment>> GetCommentsByTicketId(int ticketId, CancellationToken ct)
    {
        await using var logger = serviceProvider.GetRequiredService<IAppLogger>();
        try
        {
            var result = await ticketRepository.GetCommentsByTicketId(ticketId, ct);
            if (!result.IsError)
            {
                return result.Value;
            }
        }
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, "Database", e.StackTrace!);
        }
        return [];
    }

    public async Task<IEnumerable<History>> GetHistoryByTicketId(int ticketId, CancellationToken ct)
    {
        await using var logger = serviceProvider.GetRequiredService<IAppLogger>();
        try
        {
            var result = await ticketRepository.GetHistoryByTicketId(ticketId, ct);
            if (!result.IsError)
            {
                return result.Value;
            }
        }
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, "Database", e.StackTrace!);
        }
        return [];
    }

    public async Task AddCommentToTicket(int ticketId, string comment)
    {
        throw new NotImplementedException();
    }
}