using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Application.Interfaces;

namespace TicketManagementSystem.Application.Events.TicketEvents
{
    public record CommentAddedToTicketEvent(int TicketId, string Text, string User) : IDomainEvent
    {
        public DateTime OccuredOn { get; } = DateTime.Now;
    }
}
