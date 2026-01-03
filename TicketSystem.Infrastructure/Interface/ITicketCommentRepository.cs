using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Infrastructure.Interface;

public interface ITicketCommentRepository
{
    public Task<bool> InsertComment(TicketComment comment, CancellationToken ct);
}