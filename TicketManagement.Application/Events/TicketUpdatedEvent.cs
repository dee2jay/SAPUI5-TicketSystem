using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Application.Interfaces;

namespace TicketManagementSystem.Application.Events
{
    public class TicketUpdatedEvent : IDomainEvent
    {
        public int TicketId { get; }
        public string Property { get; }
        public string? OldValue { get; }
        public string? NewValue { get; }
        public DateTime ChangedAt { get; } = DateTime.UtcNow;
        public string ChangedBy { get; }

        public TicketUpdatedEvent(int ticketId, string property, string? oldValue, string? newValue, string changedBy)
        {
            TicketId = ticketId;
            Property = property;
            OldValue = oldValue;
            NewValue = newValue;
            ChangedBy = changedBy;
        }
    }
}
