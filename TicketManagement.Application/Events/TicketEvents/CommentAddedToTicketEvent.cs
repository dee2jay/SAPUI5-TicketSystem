using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;
using TicketManagementSystem.Application.Interfaces;

namespace TicketManagementSystem.Application.Events.TicketEvents
{
    public record CommentAddedToTicketEvent(int TicketId, string Text, string User) : IDomainEvent
    {
        public Instant OccuredOn { get; } = SystemClock.Instance.GetCurrentInstant();
    }
}
