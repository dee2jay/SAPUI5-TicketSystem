using TicketManagementSystem.Application.Events.TicketEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Application.EventHandlers.TicketEventHandlers;

public class TicketUpdatedEventHandler(IAppLogger logger) : IEventHandler<TicketUpdatedEvent>
{
    public async Task HandleAsync(TicketUpdatedEvent @event, CancellationToken ct)
    {
        try
        {
            foreach (var (property, values) in @event.Changes)
            {
                ct.ThrowIfCancellationRequested();

                var message =
                    $"[{@event.OccuredOn}] Ticket {@event.TicketId} updated | " +
                    $"{property}: '{(values.OldValue)}' → '{(values.NewValue)}' | " +
                    $"by {@event.UpdatedBy}";

                await logger.LogInfo(message);
            }
        }
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, nameof(TicketUpdatedEventHandler), e.StackTrace!);
        }
        
    }

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }
}