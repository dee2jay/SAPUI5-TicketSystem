using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Infrastructure.Persistence.Repository;

public sealed class TicketRepository(IAppLogger logger, TicketDbContext dbContext, IServiceScopeFactory serviceScopeFactory) : ITicketRepository
{
    public async Task AddTicket(Ticket ticket)
    {
        try
        {
            dbContext.Tickets.Add(ticket);
            await dbContext.SaveChangesAsync();
            await logger.LogInfo($"Ticket with ID {ticket.Id} added.");
        }
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, nameof(TicketRepository), e.StackTrace!);
        }
        
    }

    public async Task<ErrorOr<Ticket>> GetTicketById(int ticketId, CancellationToken ct)
    {
        var ticket = await dbContext.Tickets
            .Include(t =>t.Comments)
            .Include(t=>t.Attachments)
            .Include(t =>t.Histories.OrderByDescending(h => h.Timestamp))
            .FirstOrDefaultAsync(t => t.Id == ticketId, ct);
        
        if (ticket != null)
        {
            return ticket;
        }
        await logger.LogWarning($"Ticket with ID {ticketId} not found.");
        return Error.NotFound(description: $"Ticket with ID {ticketId} not found.");
    }

    public async Task UpdateTicket(Ticket ticket) 
    {
        try
        {
            await logger.LogInfo($"Ticket with ID {ticket.Id} updated.");

            try
            {
                using var scope = serviceScopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<TicketDbContext>();
                db.Tickets.Update(ticket);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                await logger.LogWarning(e.Message, e.Source);
                throw new Exception("The ticket was modified by another user. Please reload the page.");
            }
    
        }
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, nameof(TicketRepository), e.StackTrace!);
        }
        
    }

    public async  Task<ErrorOr<IEnumerable<TicketAttachment>>> GetAttachmentsByTicketId(int ticketId, CancellationToken ct)
    {
        try
        {
            var attachments = await dbContext.TicketAttachments. 
                Where(ta => ta.TicketId == ticketId).ToListAsync(ct);
            if (attachments.Count > 0)
            {
                return attachments;
            }
            return Error.NotFound(description: $"No history found for the Ticket with ID {ticketId}.");
        }
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, nameof(TicketRepository), e.StackTrace!);
        }

        return default;
    }

    public async Task<ErrorOr<IEnumerable<TicketComment>>> GetCommentsByTicketId(int ticketId, CancellationToken ct)
    {
        try
        {
            var comments = await dbContext.TicketComments.
                Where(tc => tc.TicketId == ticketId).ToListAsync(ct);
            if (comments.Count > 0)
            {
                return comments;
            }
            return Error.NotFound(description: $"No history found for the Ticket with ID {ticketId}.");
        }
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, nameof(TicketRepository), e.StackTrace!);
        }

        return default;
    }

    public async Task<ErrorOr<IEnumerable<History>>> GetHistoryByTicketId(int ticketId, CancellationToken ct)
    {
        try
        {
            var historyList = await dbContext.TicketHistories.
                Where(h => h.TicketId == ticketId).ToListAsync(ct);
            if (historyList.Count > 0)
            {
                return historyList;
            }
            return Error.NotFound(description: $"No history found for the Ticket with ID {ticketId}.");
        }
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, nameof(TicketRepository), e.StackTrace!);
        }

        return default;
    }

    public async Task<ErrorOr<IEnumerable<Ticket>>> GetAllTickets(CancellationToken ct)
    {
        var ticketList = await dbContext.Tickets
            .Include(t => t.Histories)
            .Include(t=>t.Attachments)
            .Include(t => t.Comments)
            .ToListAsync(ct);
        if (ticketList.Count > 0)
        {
            return ticketList;
        }

        return Error.NotFound(description: $"No Tickets found");
    }
}