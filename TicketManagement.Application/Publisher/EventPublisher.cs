using Microsoft.Extensions.DependencyInjection;
using TicketManagementSystem.Application.Interfaces;

namespace TicketManagementSystem.Application.Publisher;

public class EventPublisher(IEventDispatcher dispatcher) : IEventPublisher
{
    public async Task PublishEventAsync<TEvent>(TEvent @event, CancellationToken ct) where TEvent : IDomainEvent
    {
        await dispatcher.DispatchAsync(@event, ct);
    }
}