using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Application.Events;
using TicketManagement.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Logging;

namespace TicketManagement.Application.EventHandlers
{
    public class TicketCreatedEventHandler : IEventHandler<TicketCreatedEvent>
    {
        private readonly MongoLogger _logger;
        public TicketCreatedEventHandler(MongoLogger logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(TicketCreatedEvent @event)
        {
            var logEntry = new TicketChangeLog
            {
                TicketId = @event.TicketId,
                OldValue = @event.OldValue,
                NewValue = @event.NewValue,
                ChangedAt = @event.ChangedAt,
                ChangedBy = @event.ChangedBy
            };

            await _logger.LogChangeAsync(logEntry);
        }
    }
}
