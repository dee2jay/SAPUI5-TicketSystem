using ErrorOr;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Infrastructure.Persistence.Repository;

public class TicketAttachmentRepository(TicketDbContext dbContext, IAppLogger logger) : ITicketAttachmentRepository
{
    public async Task<ErrorOr<TicketAttachment>> GetTicketAttachmentById(string id, CancellationToken ct)
    {
        var attachment = await dbContext.TicketAttachments.FirstOrDefaultAsync(ta => ta.Id.ToString() ==id , ct);

        if (attachment != null)
        {
            return attachment;
        }
        await logger.LogWarning($"Ticket with ID {id} not found.");
        return Error.NotFound(description: $"Ticket attachment with ID {id} not found.");
    }

    public async Task AddAttachmentAsync(TicketAttachment attachment, CancellationToken ct)
    {
        try
        {
            dbContext.TicketAttachments.Add(attachment);
            await dbContext.SaveChangesAsync(ct);
            await logger.LogInfo($"Attachment {attachment.FileName} has been added to the Ticket with ID {attachment.TicketId}.");
        }
        catch (DbException e)
        {
            await logger.LogError(e.Message, e, e.Source, e.StackTrace!);
        }
    }
}