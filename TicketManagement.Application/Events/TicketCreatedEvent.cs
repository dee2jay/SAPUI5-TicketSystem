using TicketManagement.Application.Interfaces;
using TicketManagementSystem.Application.Interfaces;

namespace TicketManagementSystem.Application.Events;

public class TicketCreatedEvent : IDomainEvent
{
    public int TicketId { get; init; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    public string ChangedBy { get; set; } = string.Empty;
    public DateTime OccurredOn { get; } = DateTime.Now;
    
}