using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Interfaces;

public interface INotificationService : IAsyncDisposable
{
    Task NotifyUserCreatedAsync(User user);
    Task NotifyTicketCreatedAsync(Ticket ticket);
    Task NotifyTicketUpdatedAsync(Ticket ticket);
    Task NotifyCommentAddedAsync(Ticket ticket);
    Task NotifyAttachmentAddedAsync(Ticket ticket);
    Task NotifyPriorityChangedAsync(Ticket ticket);
    Task NotifyStatusChangedAsync(Ticket ticket);

}