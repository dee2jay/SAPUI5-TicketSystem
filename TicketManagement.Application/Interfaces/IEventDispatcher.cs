
using TicketManagement.Application.Interfaces;

namespace TicketManagementSystem.Application.Interfaces;

public interface IEventDispatcher
{
    Task DispatchAsync<TEvent>(TEvent @event) where TEvent : IDomainEvent;
}