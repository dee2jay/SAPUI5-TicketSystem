using NodaTime;

namespace TicketManagementSystem.Application.Interfaces;

public interface IDomainEvent
{
    public Instant OccuredOn { get; }
}