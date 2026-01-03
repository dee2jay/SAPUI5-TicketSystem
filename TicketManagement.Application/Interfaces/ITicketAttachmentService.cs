using Microsoft.AspNetCore.Http;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Interfaces;

public interface ITicketAttachmentService
{
    public Task<TicketAttachment?> AddAttachment(int ticketId, IFormFile file, User user, CancellationToken ct);
    public Task<TicketAttachment> GetAttachmentById(string attachmentId, CancellationToken ct);
}