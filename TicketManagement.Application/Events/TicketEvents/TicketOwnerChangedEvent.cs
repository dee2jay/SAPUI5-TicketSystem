using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Events.TicketEvents;

public record TicketOwnerChangedEvent(Ticket Ticket, string NewAssignedUser, string User) : IDomainEvent
{
    public DateTime OccuredOn { get; } = DateTime.Now;
}
