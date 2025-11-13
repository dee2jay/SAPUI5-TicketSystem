namespace TicketManagementSystem.Application.Interfaces;

public interface IEventPublisher
{
    Task PublishEventAsync<TEvent>(TEvent @event) where TEvent : IDomainEvent;
}