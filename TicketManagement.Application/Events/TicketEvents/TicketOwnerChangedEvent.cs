using NodaTime;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Events.TicketEvents;

public record TicketOwnerChangedEvent(Ticket Ticket, string OldAssignedUser , string NewAssignedUser, string ChangeBy) : IDomainEvent
{
    public Instant OccuredOn { get; } = SystemClock.Instance.GetCurrentInstant();
}
