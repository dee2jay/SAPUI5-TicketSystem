namespace TicketManagementSystem.Application.Interfaces;

public interface IDomainEvent
{
    public DateTime OccuredOn { get; }
}