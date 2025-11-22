using Microsoft.Extensions.DependencyInjection;
using TicketManagementSystem.Application.Events.TicketEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;
using TicketManagementSystem.Infrastructure.Persistence;

namespace TicketManagementSystem.Application.EventHandlers.TicketEventHandlers;

public class TicketUpdatedEventHandler(IAppLogger logger, INotificationService notificationService) : IEventHandler<TicketUpdatedEvent>
{
    public async Task HandleAsync(TicketUpdatedEvent @event, CancellationToken ct)
    {
        try
        {
            foreach (var message in @event.Changes.Select(change => new TicketChangeLog
                     {
                         Title = "Ticket Updated",
                         TicketId = @event.TicketId,
                         Property = change.Key,
                         OldValue = change.Value.OldValue!.ToString(),
                         NewValue = change.Value.NewValue!.ToString(),
                         ChangedAt = @event.OccuredOn,
                         ChangedBy = @event.UpdatedBy
                     }).Select(logEntry => $"Timestamp -> {logEntry.ChangedAt}, {logEntry.Title}, TicketId: {logEntry.TicketId}, " +
                                           $"OldValue: {logEntry.OldValue}, " +
                                           $"NewValue: {logEntry.NewValue}, " +
                                           $"Updated by {logEntry.ChangedBy}"))
            {
                await logger.LogInfo(message);
            }
        }
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, nameof(TicketUpdatedEventHandler), e.StackTrace!);
        }
        
    }

    public async ValueTask DisposeAsync()
    {
        await logger.DisposeAsync();
    }
}