using NodaTime;
using TicketManagementSystem.Application.Interfaces;

namespace TicketManagementSystem.Application.Events.TicketEvents;

public record AttachmentDeletedToTicketEvent(int TicketId, string AttachmentId, string User) : IDomainEvent
{
    public Instant OccuredOn { get; } = SystemClock.Instance.GetCurrentInstant();
}