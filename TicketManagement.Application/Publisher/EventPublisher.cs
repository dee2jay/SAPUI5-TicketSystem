using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TicketManagement.Application.Dispatcher;
using TicketManagement.Application.Interfaces;

namespace TicketManagement.Application.Publisher
{
    public class EventPublisher(IEventDispatcher eventDispatcher) : IEventPublisher
    {
        public async Task PublishEventAsync<TEvent>(TEvent @event) where TEvent : IDomainEvent
        {
            // Dispatch the event using the EventDispatcher
            await eventDispatcher.DispatchAsync(@event);
        }
    }
}
