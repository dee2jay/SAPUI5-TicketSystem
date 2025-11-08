using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketSystem.Domain.Models;

namespace TicketManagement.Application.Interfaces
{
    public interface ITicketService
    {
        public Task<Ticket> CreateTicketAsync(Ticket ticket, string userId);
        public Task UpdateTicketAsync(int ticketId, Ticket updated);
        public Task AssignTicketToUserAsync(int ticketId, string userId);
        public Task<IEnumerable<Ticket>> GetAllTicketsAsync();
    }
}
