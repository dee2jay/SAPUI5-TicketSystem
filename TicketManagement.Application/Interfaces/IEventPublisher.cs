namespace TicketManagementSystem.Application.Interfaces;

public interface IEventPublisher
{
    Task PublishEventAsync<TEvent>(TEvent @event, CancellationToken ct) where TEvent : IDomainEvent;
}