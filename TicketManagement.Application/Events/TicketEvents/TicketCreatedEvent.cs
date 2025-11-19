using TicketManagementSystem.Application.Interfaces;

namespace TicketManagementSystem.Application.Events.TicketEvents;

public class TicketCreatedEvent : IDomainEvent
{
    public int TicketId { get; init; }
    public string? Value { get; set; }
    public string ChangedBy { get; set; } = string.Empty;
    public DateTime OccuredOn { get; } = DateTime.UtcNow;
}