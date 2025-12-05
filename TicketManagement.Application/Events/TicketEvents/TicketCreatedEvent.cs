using NodaTime;
using TicketManagementSystem.Application.Interfaces;

namespace TicketManagementSystem.Application.Events.TicketEvents;

public record TicketCreatedEvent(int TicketId, string Title, string CreatedBy, string AssignedTo) : IDomainEvent
{
    public Instant OccuredOn { get; } = SystemClock.Instance.GetCurrentInstant();
}