using NodaTime;
using TicketManagementSystem.Application.Interfaces;

namespace TicketManagementSystem.Application.Events.UserEvents;

public record UserCreatedEvent(string Vorname, string Name, string Username, string Email, Instant Timestamp)
    : IDomainEvent
{
    public Instant OccuredOn { get; } = SystemClock.Instance.GetCurrentInstant();
}
