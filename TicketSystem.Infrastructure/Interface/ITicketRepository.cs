using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;
using TicketManagementSystem.Domain.Models;
using TicketSystem.Domain.Models;

namespace TicketManagementSystem.Infrastructure.Interface;

public interface ITicketRepository
{
    Task AddTicket(Ticket ticket);
    Task<ErrorOr<Ticket>> GetTicketById(int ticketId);
    Task<ErrorOr<IEnumerable<Ticket>>> GetAllTickets();
    Task UpdateTicket(Ticket ticket);


}