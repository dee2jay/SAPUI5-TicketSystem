using Microsoft.Extensions.DependencyInjection;
using TicketManagementSystem.Application.Events.TicketEvents;
using TicketManagementSystem.Application.Interfaces;

namespace TicketManagementSystem.Application.Dispatcher;

public class EventDispatcher : IEventDispatcher
{
    private IServiceProvider _serviceProvider;
    private readonly IEnumerable<IEventHandler<TicketUpdatedEvent>> _ticketUpdatedEventHandlers;
    public EventDispatcher(IServiceProvider serviceProvider, IEnumerable<IEventHandler<TicketUpdatedEvent>> ticketUpdatedEventHandlers)
    {
        _serviceProvider = serviceProvider;
        _ticketUpdatedEventHandlers = ticketUpdatedEventHandlers;
    }

    public async Task DispatchAsync<TEvent>(TEvent @event, CancellationToken ct) where TEvent : IDomainEvent
    {
        // Implementation for dispatching events to appropriate handlers goes here.            
        if (@event is TicketUpdatedEvent ticketUpdatedEvent)
        {
            foreach (var ticketUpdatedEventHandler in _ticketUpdatedEventHandlers)
            {
                await ticketUpdatedEventHandler.HandleAsync(ticketUpdatedEvent, ct);
            }
        }
        var handlers = _serviceProvider.GetServices<IEventHandler<TEvent>>();
        foreach (var handler in handlers)
        {
            await handler.HandleAsync(@event, ct);
        }
    }

}