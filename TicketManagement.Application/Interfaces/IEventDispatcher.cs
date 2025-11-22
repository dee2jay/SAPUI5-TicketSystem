
namespace TicketManagementSystem.Application.Interfaces;

public interface IEventDispatcher
{
    Task DispatchAsync<TEvent>(TEvent @event, CancellationToken ct) where TEvent : IDomainEvent;
}