using Microsoft.Extensions.DependencyInjection;
using TicketManagement.Application.Interfaces;
using TicketManagementSystem.Application.Interfaces;

namespace TicketManagementSystem.Application.Dispatcher;

public class EventDispatcher : IEventDispatcher
{
    private IServiceProvider _serviceProvider;
    public EventDispatcher(IServiceProvider serviceProvider) 
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchAsync<TEvent>(TEvent @event) where TEvent : IDomainEvent
    {
        // Implementation for dispatching events to appropriate handlers goes here.            
        var handlers = _serviceProvider.GetServices<IEventHandler<TEvent>>();
        foreach (var handler in handlers)
        {
            await handler.HandleAsync(@event);
        }
    }
}