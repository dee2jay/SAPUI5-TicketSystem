using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Application.Events;
using TicketManagementSystem.Application.Events.TicketEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Enums;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Application.Services;

public class TicketService : ITicketService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ITicketRepository _ticketRepository;
    private readonly IMapper _mapper;
    private readonly IEventPublisher _eventPublisher;
    
    

    public TicketService( ITicketRepository ticketRepository,  IMapper mapper, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _ticketRepository = ticketRepository;
        _mapper = mapper;
        _eventPublisher = _serviceProvider.GetRequiredService<IEventPublisher>();
    }

    public async Task<IEnumerable<Ticket>> GetAllTicketsAsync(CancellationToken ct)
    {
        await using var logger = _serviceProvider.GetRequiredService<IAppLogger>();
        try
        {
            ct.ThrowIfCancellationRequested();
            
            var tickets = await _ticketRepository.GetAllTickets();
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
        await using var logger = _serviceProvider.GetRequiredService<IAppLogger>();
        try
        {
            // Implementation for assigning ticket to user goes here.
            var ticket = await _ticketRepository.GetTicketById(ticketId, ct);

            if (ticket.IsError)
            {
                await logger.LogWarning($"Ticket with ID {ticketId} not found.", nameof(TicketService));
                return;
            }
            ticket.Value.AssignedTo = userId;
            ticket.Value.UpdatedAt = DateTime.Now;
            await _ticketRepository.UpdateTicket(ticket.Value);
            await  logger.LogInfo($"Ticket {ticketId} assigned to user {userId} at {DateTime.Now}", nameof(TicketService));
        }
        catch (Exception ex)
        {
            if (ex.StackTrace != null)
                await logger.LogError("Error assigning ticket to user", ex, nameof(TicketService), ex.StackTrace);
        }
    }

    public async Task<Ticket> CreateTicketAsync(TicketDto dto, CancellationToken ct)
    {
        await using var logger = _serviceProvider.GetRequiredService<IAppLogger>();
        await using var userService = _serviceProvider.GetRequiredService<IUserService>();
        try
        {
            // Implementation for creating a ticket goes here.
            ct.ThrowIfCancellationRequested();

            var ticket = _mapper.Map<Ticket>(dto);
            ticket.Status = TicketStatus.New;
            ticket.Priority = TicketPriority.Normal;
            ticket.UpdatedAt = ticket.CreatedAt;
            
            var user = await userService.GetCurrentUser();
            if (user == null)
            {
                ticket.Author = string.Empty;
            }
            else
            {
                ticket.Author = user;
            }
            await _ticketRepository.AddTicket(ticket);
                
            var ticketCreatedEvent = new TicketCreatedEvent{
                TicketId = ticket.Id,
                Value = ticket.Title,
                ChangedBy = ticket.Author
            };
            await _eventPublisher.PublishEventAsync(ticketCreatedEvent);
            return ticket;
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

    public async Task UpdateTicketAsync(int ticketId, TicketUpdateDto dto, CancellationToken ct)
    {
        await using var logger = _serviceProvider.GetRequiredService<IAppLogger>();
        await using var userService = _serviceProvider.GetRequiredService<IUserService>();

        var user = await userService.GetCurrentUser();
        var propertyList = new List<string>();

        var result = await _ticketRepository.GetTicketById(ticketId, ct);
        if (result.IsError || result.Value == null)
        {
            throw new Exception($"Ticket with Id {ticketId} not found");
        }

        var currentTicket = result.Value;

        if (dto.Priority != currentTicket.Priority)
        {
            await _eventPublisher.PublishEventAsync(
                new TicketPriorityChangedEvent(currentTicket, dto.Priority, user));
            propertyList.Add("Priority");
            currentTicket.Priority = dto.Priority;
        }

        if (dto.Status != currentTicket.Status)
        {
            await _eventPublisher.PublishEventAsync(
                new TicketStatusChangedEvent(currentTicket, user, dto.Status)
                );
            propertyList.Add("Status");
            currentTicket.Status = dto.Status;
        }

        if ( dto.AssignedTo != null && dto.AssignedTo != currentTicket.AssignedTo)
        {
            await _eventPublisher.PublishEventAsync(
                new TicketOwnerChangedEvent(currentTicket, dto.AssignedTo, user)
                );
            propertyList.Add("AssignTo");
            currentTicket.AssignedTo = dto.AssignedTo;
        }

        if (dto.NewComments != null)
        {
            foreach (var commentDto in dto.NewComments)
            {
                ct.ThrowIfCancellationRequested();
                var comment = new TicketComment { Text = commentDto.Text, CreatedAt = DateTime.Now, Author = commentDto.Author };
                currentTicket.Comments.Add(comment);
                propertyList.Add("Comments");
                await _eventPublisher.PublishEventAsync(
                    new CommentAddedToTicketEvent(ticketId, comment.Text!, user));
            }
        }

        if (dto.NewAttachments != null)
        {
            foreach (var attachmentDto in dto.NewAttachments)
            {
                ct.ThrowIfCancellationRequested();
                var attachment = new TicketAttachment { FileName = attachmentDto.FileName, Url = attachmentDto.Url, Data = attachmentDto.Data, UploadedAt = DateTime.Now };
                currentTicket.Attachments.Add(attachment);
                propertyList.Add("Attachment");
                await _eventPublisher.PublishEventAsync(
                    new AttachmentAddedToTicketEvent(ticketId, attachment.FileName, user));
            }
        }

        await _ticketRepository.UpdateTicket(currentTicket);

        await _eventPublisher.PublishEventAsync(new TicketUpdatedEvent(ticketId, propertyList,user));
    }

    public async Task<Ticket?> GetTicketById(int ticketId, CancellationToken ct)
    {
        await using var logger = _serviceProvider.GetRequiredService<IAppLogger>();
        try
        {
            var existingTicket = await _ticketRepository.GetTicketById(ticketId, ct);
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
}