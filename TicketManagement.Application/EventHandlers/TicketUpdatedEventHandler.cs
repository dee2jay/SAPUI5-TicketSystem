using TicketManagementSystem.Application.Events;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Application.EventHandlers;

public class TicketUpdatedEventHandler(IAppLogger logger) : IEventHandler<TicketUpdatedEvent>
{
    public async Task HandleAsync(TicketUpdatedEvent @event)
    {
        var logEntry = new TicketChangeLog
        {
            Title = $"Ticket Updated: {@event.Property}",
            TicketId = @event.TicketId,
            Property = @event.Property,
            OldValue = @event.OldValue,
            NewValue = @event.NewValue,
            ChangedAt = @event.ChangedAt,
            ChangedBy = @event.ChangedBy
        };

        var message =
            $"{logEntry.Title}: TicketId: {logEntry.TicketId}, OldValue: {logEntry.OldValue}, NewValue: {logEntry.NewValue}, TimeStamp: {logEntry.ChangedAt}, User: {logEntry.ChangedBy}";

        await logger.LogInfo(message);
        //send Mail
        //log send mail
    }
}