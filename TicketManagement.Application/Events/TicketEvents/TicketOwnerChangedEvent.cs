using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Events.TicketEvents;

public record TicketOwnerChangedEvent(Ticket Ticket, string OldAssignedUser , string NewAssignedUser, string ChangeBy) : IDomainEvent
{
    public Instant OccuredOn { get; } = SystemClock.Instance.GetCurrentInstant();
}
