using NodaTime;
using TicketManagementSystem.Application.Interfaces;

namespace TicketManagementSystem.Application.Events.UserEvents;

public record UserLoginEvent(string Email, string Username) : IDomainEvent
{
    public Instant OccuredOn { get; } = SystemClock.Instance.GetCurrentInstant();
}