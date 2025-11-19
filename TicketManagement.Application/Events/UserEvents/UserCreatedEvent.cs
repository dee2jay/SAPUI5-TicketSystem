using TicketManagementSystem.Application.Interfaces;

namespace TicketManagementSystem.Application.Events.UserEvents;

public record UserCreatedEvent(string Vorname, string Name, string Username, string Email, DateTime Timestamp)
    : IDomainEvent
{
    public DateTime OccuredOn { get; } = DateTime.UtcNow;
}
