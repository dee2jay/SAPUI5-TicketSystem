using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Infrastructure.Persistence.Repository;

public sealed class TicketRepository(IAppLogger logger, TicketDbContext dbcontext) : ITicketRepository
{

    public async Task AddTicket(Ticket ticket)
    {
        try
        {
            dbcontext.Tickets.Add(ticket);
            await dbcontext.SaveChangesAsync();
            await logger.LogInfo($"Ticket with ID {ticket.Id} added.");
        }
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, nameof(TicketRepository), e.StackTrace!);
        }
        
    }

    public async Task<ErrorOr<Ticket>> GetTicketById(int ticketId, CancellationToken ct)
    {
        var ticket = await dbcontext.Tickets
            .Include(t =>t.Comments)
            .Include(t=>t.Attachments)
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
            dbcontext.Tickets.Update(ticket);

            try
            {
                await dbcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new Exception("The ticket was modified by another user. Please reload the page.");
            }
            
    
        }
        catch (Exception e)
        {
            logger.LogError(e.Message, e, nameof(TicketRepository), e.StackTrace!);
        }
        
    }
    public async Task<ErrorOr<IEnumerable<Ticket>>> GetAllTickets()
    {
        var ticketList = await dbcontext.Tickets.ToListAsync();
        if (ticketList.Count > 0)
        {
            return ticketList;
        }

        return Error.NotFound(description: $"No Tickets found");
    }
}