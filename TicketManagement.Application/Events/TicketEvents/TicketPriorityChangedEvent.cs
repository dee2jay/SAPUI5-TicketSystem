using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Enums;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Events.TicketEvents;

public record TicketPriorityChangedEvent(Ticket Ticket, TicketPriority NewPriority, string User) : IDomainEvent
{
    public DateTime OccuredOn { get; } = DateTime.Now;
}