using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Infrastructure.Persistence.Repository;

public sealed class TicketRepository(ILogger<TicketRepository> logger, TicketDbContext dbcontext) : ITicketRepository
{
    private readonly ILogger<ITicketRepository> _logger = logger;
    private readonly TicketDbContext _dbcontext = dbcontext;

    public async Task AddTicket(Ticket ticket)
    {
        _dbcontext.Tickets.Add(ticket);
        await _dbcontext.SaveChangesAsync();
        _logger.LogInformation($"Ticket with ID {ticket.Id} added.");
    }

    public async Task<ErrorOr<Ticket>> GetTicketById(int ticketId)
    {
        var ticket = await _dbcontext.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
        if (ticket != null)
        {
            return ticket;
        }
        _logger.LogWarning($"Ticket with ID {ticketId} not found.");
        return Error.NotFound(description: $"Ticket with ID {ticketId} not found.");
    }

    public async Task UpdateTicket(Ticket ticket) 
    { 
        _dbcontext.Tickets.Update(ticket);
        await _dbcontext.SaveChangesAsync();
        _logger.LogInformation($"Ticket with ID {ticket.Id} updated.");
    }
    public async Task<ErrorOr<IEnumerable<Ticket>>> GetAllTickets()
    {
        var ticketList = await _dbcontext.Tickets.ToListAsync();
        if (ticketList.Count > 0)
        {
            return ticketList;
        }

        return Error.NotFound(description: $"No Tickets found");
    }
}