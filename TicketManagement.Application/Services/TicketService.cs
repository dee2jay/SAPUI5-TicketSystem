using AutoMapper;
using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Application.Events;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Enums;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;
using TicketSystem.Domain.Models;

namespace TicketManagementSystem.Application.Services;

public class TicketService : ITicketService
{
    private readonly IAppLogger _logger;
    private readonly ITicketRepository _ticketRepository;
    private readonly IEventPublisher _eventPublisher;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public TicketService( ITicketRepository ticketRepository, IEventPublisher eventPublisher, IAppLogger logger, IMapper mapper, IUserService userService)
    {
        _ticketRepository = ticketRepository;
        _eventPublisher = eventPublisher;
        _logger = logger;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
    {
        try
        {
            var tickets = await _ticketRepository.GetAllTickets();
            if (!tickets.IsError)
            {
                return tickets.Value;
            }
            await _logger.LogWarning("No tickets found.", nameof(TicketService));
        }
        catch (Exception e)
        {
            await _logger.LogError(e.Message, e, "Database", e.StackTrace!);
        }

        return [];
    }
    public async Task AssignTicketToUserAsync(int ticketId, string userId)
    {
        try
        {
            // Implementation for assigning ticket to user goes here.
            var ticket = await _ticketRepository.GetTicketById(ticketId);

            if (ticket.IsError)
            {
                await _logger.LogWarning($"Ticket with ID {ticketId} not found.", nameof(TicketService));
                return;
            }
            ticket.Value.AssignedTo = userId;
            ticket.Value.UpdatedAt = DateTime.Now;
            await _ticketRepository.UpdateTicket(ticket.Value);
            await  _logger.LogInfo($"Ticket {ticketId} assigned to user {userId} at {DateTime.Now}", nameof(TicketService));
        }
        catch (Exception ex)
        {
            if (ex.StackTrace != null)
                await _logger.LogError("Error assigning ticket to user", ex, nameof(TicketService), ex.StackTrace);
        }
    }

    public async Task<Ticket> CreateTicketAsync(TicketDto dto)
    {
        try
        {
            // Implementation for creating a ticket goes here.
            var ticket = _mapper.Map<Ticket>(dto);
            ticket.Status = TicketStatus.New;
            ticket.Priority = TicketPriority.Normal;
            ticket.UpdatedAt = ticket.CreatedAt;
            ticket.Author = await _userService.GetCurrentUser();
            await _ticketRepository.AddTicket(ticket);
                
            var ticketCreatedEvent = new TicketCreatedEvent{
                TicketId = ticket.Id,
                OldValue = null,
                NewValue = ticket.Title,
                ChangedAt = ticket.CreatedAt,
                ChangedBy = ticket.Author
            };
            await _eventPublisher.PublishEventAsync(ticketCreatedEvent);
            return ticket;
        }
        catch (Exception ex)
        {
            if (ex.StackTrace != null)
            {
                await _logger.LogError("Error creating ticket", ex, nameof(TicketService), ex.StackTrace);
            }
            return null!;
        }
    }

    public async Task UpdateTicketAsync(int ticketId, Ticket updated)
    {
        try
        {
            // Implementation for updating ticket status goes here.
            var existingTicket = await _ticketRepository.GetTicketById(ticketId);
                
            if (existingTicket.IsError)
            {
                await _logger.LogWarning($"Ticket with ID {ticketId} not found.", nameof(TicketService));
                return;
            }
            await RaiseChange(existingTicket.Value, updated, ticketId);

            await RaiseChangeCollectionChangeAsync(
                nameof(existingTicket.Value.Attachments),
                existingTicket.Value.Attachments,
                updated.Attachments,
                ticketId,
                a => a.Id // Assuming TicketAttachment has an Id property
            );
                
            await RaiseChangeCollectionChangeAsync(
                nameof(existingTicket.Value.Comments),
                existingTicket.Value.Comments,
                updated.Comments,
                ticketId,
                c => c.Id // Assuming TicketComment has an Id property
            );

            existingTicket.Value.Title = updated.Title;
            existingTicket.Value.Description = updated.Description;
            existingTicket.Value.Status = updated.Status;
            existingTicket.Value.Priority = updated.Priority;
            existingTicket.Value.Category = updated.Category;
            existingTicket.Value.AssignedTo = updated.AssignedTo;
            existingTicket.Value.UpdatedAt = DateTime.Now;
            existingTicket.Value.Attachments = updated.Attachments;
            existingTicket.Value.Comments = updated.Comments;

            await _ticketRepository.UpdateTicket(existingTicket.Value);
        }
        catch (Exception ex)
        {
            if (ex.StackTrace != null)
                await _logger.LogError("Error updating ticket status", ex, nameof(TicketService), ex.StackTrace);
        }
    }

    public async Task CloseTicketAsync(int ticketId)
    {
        try
        {
            // Implementation for closing ticket goes here.
            var ticket = await _ticketRepository.GetTicketById(ticketId);
            if (ticket.IsError)
            {
                await _logger.LogWarning($"Ticket with ID {ticketId} not found.", nameof(TicketService));
                return;
            }
            ticket.Value.Status = TicketStatus.Closed;
            ticket.Value.UpdatedAt = DateTime.Now;
            await _ticketRepository.UpdateTicket(ticket.Value);
            await _logger.LogInfo($"Ticket {ticketId} closed at {DateTime.Now}", nameof(TicketService));
        }
        catch (Exception ex)
        {
            if (ex.StackTrace != null)
                await _logger.LogError("Error closing ticket", ex, nameof(TicketService), ex.StackTrace);
        }
    }

    public async Task AddCommentToTicketAsync(int ticketId, TicketComment comment)
    {
        try
        {
            var ticket = await _ticketRepository.GetTicketById(ticketId);
            if (ticket.IsError)
            {
                await _logger.LogWarning($"Ticket with ID {ticketId} not found.", nameof(TicketService));
                return;
            }
            ticket.Value.Comments.Add(comment);
            ticket.Value.UpdatedAt = DateTime.Now;
            await _ticketRepository.UpdateTicket(ticket.Value);
            await _logger.LogInfo($"Comment added to ticket {ticketId} at {DateTime.Now}", nameof(TicketService));
        }
        catch (Exception ex)
        {
            if (ex.StackTrace != null)
                await _logger.LogError("Error adding comment to ticket", ex, nameof(TicketService), ex.StackTrace);
        }
    }

    public async Task AddAttachmentToTicketAsync(int ticketId, TicketAttachment attachment)
    {
        try
        {
            var ticket = await _ticketRepository.GetTicketById(ticketId);
            if (ticket.IsError)
            {
                await _logger.LogWarning($"Ticket with ID {ticketId} not found.", nameof(TicketService));
                return;
            }
            ticket.Value.Attachments.Add(attachment);
            ticket.Value.UpdatedAt = DateTime.Now;
            await _ticketRepository.UpdateTicket(ticket.Value);
        }
        catch (Exception ex)
        {
            if (ex.StackTrace != null)
                await _logger.LogError("Error adding attachment to ticket", ex, nameof(TicketService),
                    ex.StackTrace);
        }
    }
        
    private async Task RaiseChangeCollectionChangeAsync<T>(string collectionName,
        IEnumerable<T>? oldCollection,
        IEnumerable<T>? newCollection,
        int ticketId,
        Func<T, object> keySelector)
    {
        if (oldCollection == null && newCollection == null)
        {
            return;
        }

        var oldSet = oldCollection?.ToDictionary(keySelector);
        var newSet = newCollection?.ToDictionary(keySelector);
        List<T> updatedItems = [.. newSet?.Values.Where(item => oldSet!.ContainsKey(keySelector(item)) && !item.Equals(oldSet[keySelector(item)]))];

        foreach (var item in newSet.Values)
        {
            if (oldSet != null && !oldSet.ContainsKey(keySelector(item)))
            {
                await _eventPublisher.PublishEventAsync(new CollectionChangedEvent<T>(ticketId, collectionName, "Added", item, Environment.UserName));
            }
        }

        foreach (var item in oldSet!.Values)
        {
            if (!newSet.ContainsKey(keySelector(item)))
            {
                await _eventPublisher.PublishEventAsync(new CollectionChangedEvent<T>(ticketId, collectionName, "Removed", item, Environment.UserName));
            }
        }

        foreach (var item in updatedItems)
        {
            if (!newSet.ContainsKey(keySelector(item)))
            {
                await _eventPublisher.PublishEventAsync(new CollectionChangedEvent<T>(ticketId, collectionName, "Updated", item, Environment.UserName));
            }
        }
    }

    private async Task RaiseChange(Ticket oldTicket, Ticket newTicket, int ticketId)
    {
        async Task RaiseAsync(string property, string? oldValue, string? newValue)
        {
            if (oldValue != newValue)
            {
                var evt = new TicketUpdatedEvent(
                    ticketId,
                    property,
                    oldValue,
                    newValue,
                    Environment.UserName
                );
                await _eventPublisher.PublishEventAsync(evt);
            }                
        }

        await RaiseAsync(nameof(Ticket.Title), oldTicket.Title, newTicket.Title);
        await RaiseAsync(nameof(Ticket.Status), oldTicket.Status.ToString(), newTicket.Status.ToString());
        await RaiseAsync(nameof(Ticket.Priority), oldTicket.Priority.ToString().ToString(), newTicket.Priority.ToString());
        await RaiseAsync(nameof(Ticket.Category), oldTicket.Category, newTicket.Category);
        await RaiseAsync(nameof(Ticket.AssignedTo), oldTicket.AssignedTo, newTicket.AssignedTo);
    }
}