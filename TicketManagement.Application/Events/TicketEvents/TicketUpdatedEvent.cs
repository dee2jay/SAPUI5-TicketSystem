using TicketManagementSystem.Application.Interfaces;

namespace TicketManagementSystem.Application.Events.TicketEvents;

public record TicketUpdatedEvent(int TicketId, List<string> PropertyList,string User) : IDomainEvent
{
    public DateTime OccuredOn { get; } = DateTime.Now;
}
