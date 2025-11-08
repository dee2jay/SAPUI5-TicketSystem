using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Application.Interfaces;

namespace TicketManagement.Application.Dispatcher
{
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

}
