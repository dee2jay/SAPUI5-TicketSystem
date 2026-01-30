using NodaTime;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Events.TicketEvents;

public record DueDateAddedToTicketEvent(Ticket Ticket, string DueDate, string ChangeBy) : IDomainEvent
{
    public Instant OccuredOn { get; }
}