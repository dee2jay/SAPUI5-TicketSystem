using TicketManagementSystem.Application.Interfaces;

namespace TicketManagementSystem.Application.Events.TicketEvents;

public record TicketUpdatedEvent(int TicketId, string UpdatedBy, Dictionary<string, (object? OldValue, object? NewValue)> Changes) : IDomainEvent
{
    public DateTime OccuredOn { get; } = DateTime.Now;
}
