using TicketManagementSystem.Application.Events.TicketEvents;
using TicketManagementSystem.Application.Events.UserEvents;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Interfaces;

public interface INotificationService : IAsyncDisposable 
{
    Task NotifyTicketCreatedAsync(TicketCreatedEvent evt, CancellationToken ct);
    Task NotifyUserCreatedAsync(UserCreatedEvent evt, CancellationToken ct);
    Task NotifyTicketUpdatedAsync(Ticket ticket);
    Task NotifyCommentAddedAsync(Ticket ticket);
    Task NotifyAttachmentAddedAsync(Ticket ticket);
    Task NotifyPriorityChangedAsync(Ticket ticket);
    Task NotifyStatusChangedAsync(TicketStatusChangedEvent evt, CancellationToken ct);
    Task NotifyTicketOwnerChanged(TicketOwnerChangedEvent evt, CancellationToken ct);
    Task NotifyDueDateAdded(DueDateAddedToTicketEvent @event, CancellationToken ct);
}