using Microsoft.Extensions.DependencyInjection;
using TicketManagementSystem.Application.Events.TicketEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;
using TicketManagementSystem.Infrastructure.Persistence;

namespace TicketManagementSystem.Application.EventHandlers.TicketEventHandlers;

public class TicketUpdatedEventHandler(IAppLogger logger, IServiceProvider serviceProvider) : IEventHandler<TicketUpdatedEvent>
{
    public async Task HandleAsync(TicketUpdatedEvent @event)
    {
        try
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
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, nameof(TicketUpdatedEventHandler), e.StackTrace!);
        }
        
    }
}