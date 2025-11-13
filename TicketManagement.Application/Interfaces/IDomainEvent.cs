namespace TicketManagementSystem.Application.Interfaces;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}