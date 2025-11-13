using TicketManagementSystem.Application.Events;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;
using TicketManagementSystem.Infrastructure.Logging;

namespace TicketManagementSystem.Application.EventHandlers;

public class TicketCreatedEventHandler : IEventHandler<TicketCreatedEvent>
{
    private readonly IAppLogger _logger;
    public TicketCreatedEventHandler(MongoLogger logger)
    {
        _logger = logger;
    }

    public async Task HandleAsync(TicketCreatedEvent @event)
    {
        var logEntry = new TicketChangeLog
        {
            Title = @event.NewValue ?? "New Ticket Created",
            TicketId = @event.TicketId,
            OldValue = @event.OldValue,
            NewValue = @event.NewValue,
            ChangedAt = @event.ChangedAt,
            ChangedBy = @event.ChangedBy
        };
        var message =
            $"{logEntry.Title}: TicketId: {logEntry.TicketId}, TimeStamp: {logEntry.ChangedAt}, User: {logEntry.ChangedBy}";

        await _logger.LogInfo(message);
        //send Mail
        //log send mail
    }
}