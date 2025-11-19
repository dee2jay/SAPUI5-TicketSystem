using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Application.Interfaces;

namespace TicketManagementSystem.Application.Events.TicketEvents;

public record AttachmentAddedToTicketEvent(int TicketId, string AttachmentName,string User) : IDomainEvent
{
    public DateTime OccuredOn { get; } = DateTime.Now;
}
    
