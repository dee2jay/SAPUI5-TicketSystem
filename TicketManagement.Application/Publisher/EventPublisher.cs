using Microsoft.Extensions.DependencyInjection;
using TicketManagementSystem.Application.Interfaces;

namespace TicketManagementSystem.Application.Publisher;

public class EventPublisher : IEventPublisher
{
    private readonly IServiceProvider _serviceProvider;

    public EventPublisher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task PublishEventAsync<TEvent>(TEvent @event) where TEvent : IDomainEvent
    {
        var handlers = _serviceProvider.GetServices<IEventHandler<TEvent>>();
        foreach (var handler in handlers)
        {
            await handler.HandleAsync(@event);
        }
    }
}