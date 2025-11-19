using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Enums;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Events.TicketEvents;

public record TicketStatusChangedEvent(Ticket Ticket, string User, TicketStatus NewStatus) : IDomainEvent
{
    public DateTime OccuredOn { get; } = DateTime.UtcNow;
}