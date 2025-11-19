using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Infrastructure.Interface;

public interface ITicketRepository
{
    Task AddTicket(Ticket ticket);
    Task<ErrorOr<Ticket>> GetTicketById(int ticketId, CancellationToken ct);
    Task<ErrorOr<IEnumerable<Ticket>>> GetAllTickets();
    Task UpdateTicket(Ticket ticket);


}