using Microsoft.Extensions.DependencyInjection;
using TicketManagementSystem.Application.Interfaces;

namespace TicketManagementSystem.Application.Publisher;

public class EventPublisher : IEventPublisher
{
    private readonly IEventDispatcher _dispatcher;

    public EventPublisher(IEventDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public async Task PublishEventAsync<TEvent>(TEvent @event, CancellationToken ct) where TEvent : IDomainEvent
    {
        await _dispatcher.DispatchAsync(@event, ct);
    }
}