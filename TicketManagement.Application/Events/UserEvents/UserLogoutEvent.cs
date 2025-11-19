using TicketManagementSystem.Application.Interfaces;

namespace TicketManagementSystem.Application.Events.UserEvents;

public record UserLogoutEvent(string Email, string Username) : IDomainEvent
{
    public DateTime OccuredOn { get; } = DateTime.UtcNow;
}