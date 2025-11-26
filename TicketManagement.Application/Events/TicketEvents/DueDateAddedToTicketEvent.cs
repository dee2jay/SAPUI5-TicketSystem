using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Events.TicketEvents;

public record DueDateAddedToTicketEvent(Ticket Ticket, string DueDate, string ChangeBy) : IDomainEvent
{
    public DateTime OccuredOn { get; }
}