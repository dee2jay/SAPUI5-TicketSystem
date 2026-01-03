using Microsoft.AspNetCore.Http;
using NodaTime;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Application.Services;

public class TicketAttachmentService(ITicketAttachmentRepository attachmentRepository, IAppLogger logger, IEventPublisher eventPublisher) : ITicketAttachmentService
{
    public async Task<TicketAttachment?> AddAttachment(int ticketId, IFormFile file, User user, CancellationToken ct)
    {
        try
        {
            var attachment = new TicketAttachment
            {
                Id = Guid.NewGuid(),
                TicketId = ticketId,
                FileName = file.FileName,
                UploadedAt = SystemClock.Instance.GetCurrentInstant(),
                UserId = user.Id
            };

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms, ct);
            attachment.Data = ms.ToArray();
            attachment.Url = $"/api/tickets/{ticketId}/attachment/get/{attachment.Id}";

            await attachmentRepository.AddAttachmentAsync(attachment, ct);

            return attachment;

        }
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, e.Source, e.StackTrace!);
            return null;
        }
    }

    public async Task<TicketAttachment> GetAttachmentById(string attachmentId, CancellationToken ct)
    {
        try
        {
            var result = await attachmentRepository.GetTicketAttachmentById(attachmentId, ct);
            if (!result.IsError)
            {
                return result.Value;
            }
        }
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, e.Source, e.StackTrace!);
                
        }
        return null!;
    }
}