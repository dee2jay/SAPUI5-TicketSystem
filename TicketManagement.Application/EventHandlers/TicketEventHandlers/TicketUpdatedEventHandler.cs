using TicketManagementSystem.Application.Events.TicketEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Application.EventHandlers.TicketEventHandlers;

public class TicketUpdatedEventHandler(IAppLogger logger) : IEventHandler<TicketUpdatedEvent>
{
    public async Task HandleAsync(TicketUpdatedEvent @event)
    {
        var logEntry = new TicketChangeLog
        {
            Title = "Ticket Updated",
            TicketId = @event.TicketId,
            Property = nameof(@event.PropertyList),
            Value = string.Join(";", @event.PropertyList.Select(p => p)),
            ChangedAt = @event.OccuredOn,
            ChangedBy = @event.User
        };

        var message =
            $"Timestamp -> {logEntry.ChangedAt}, {logEntry.Title}, TicketId: {logEntry.TicketId}, " +
            $"Value: {logEntry.Value}, User: {logEntry.ChangedBy}";

        await logger.LogInfo(message);
    }
}