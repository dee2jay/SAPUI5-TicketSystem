using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Domain.Models;
using TicketSystem.Domain.Models;

namespace TicketManagement.Application.Interfaces
{
    public interface ITicketService
    {
        public Task<Ticket> CreateTicketAsync(TicketDto dto);
        public Task UpdateTicketAsync(int ticketId, Ticket updated);
        public Task CloseTicketAsync(int ticketId);
        public Task AssignTicketToUserAsync(int ticketId, string userId);
        public Task<IEnumerable<Ticket>> GetAllTicketsAsync();
        public Task AddCommentToTicketAsync(int ticketId, TicketComment comment);
        public Task AddAttachmentToTicketAsync(int ticketId, TicketAttachment attachment);
    }
}
