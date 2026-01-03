using ErrorOr;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Infrastructure.Interface;

public interface ITicketAttachmentRepository
{
    Task<ErrorOr<TicketAttachment>> GetTicketAttachmentById(string id, CancellationToken ct);
    Task AddAttachmentAsync(TicketAttachment  attachment, CancellationToken ct);
}