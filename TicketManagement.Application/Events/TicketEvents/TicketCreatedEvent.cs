using TicketManagementSystem.Application.Interfaces;

namespace TicketManagementSystem.Application.Events.TicketEvents;

public record TicketCreatedEvent(int TicketId, string Title, string CreatedBy, string AssignedTo) : IDomainEvent
{
    public DateTime OccuredOn { get; } = DateTime.UtcNow;
}