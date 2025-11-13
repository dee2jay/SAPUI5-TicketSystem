namespace TicketManagementSystem.Application.Interfaces;

public interface IEventHandler<in TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent @event);
}