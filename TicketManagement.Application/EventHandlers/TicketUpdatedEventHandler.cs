using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Application.Interfaces;
using TicketManagementSystem.Application.Events;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Logging;

namespace TicketManagement.Application.EventHandlers
{
    public class TicketUpdatedEventHandler(MongoLogger logger) : IEventHandler<TicketUpdatedEvent>
    {
        public async Task HandleAsync(TicketUpdatedEvent @event)
        {
            var logEntry = new TicketChangeLog
            {
                TicketId = @event.TicketId,
                Property = @event.Property,
                OldValue = @event.OldValue,
                NewValue = @event.NewValue,
                ChangedAt = @event.ChangedAt,
                ChangedBy = @event.ChangedBy
            };

            await logger.LogChangeAsync(logEntry);
        }
    }
}
