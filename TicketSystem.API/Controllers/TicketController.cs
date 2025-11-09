using Microsoft.AspNetCore.Mvc;
using TicketManagement.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;
using TicketSystem.Domain.Models;

namespace TicketManagementSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet(Name = "GetTickets")]
        public async Task<IEnumerable<Ticket>> GetTickets()
        {
            return await _ticketService.GetAllTicketsAsync();
        }

        [HttpPost(Name = "AddTicket")]
        public void AddTicket([FromBody] Ticket ticket, string user)
        {
            ticket.Author = user;
            _ticketService.CreateTicketAsync(ticket, user);
        }

        [HttpPut("{ticketId}/assign/{userId}", Name = "AssignTicket")]
        public void AssignTicket(int ticketId, string userId)
        {
            _ticketService.AssignTicketToUserAsync(ticketId, userId);
        }

        [HttpPut("{ticketId}/close", Name = "CloseTicket")]
        public void CloseTicket(int ticketId)
        {
            _ticketService.CloseTicketAsync(ticketId);
        }

        [HttpPut("{ticketId}/addComment", Name = "CommentTicket")]
        public void CommentTicket(int ticketId, [FromBody] TicketComment comment)
        {
             _ticketService.AddCommentToTicketAsync(ticketId, comment);
        }

        [HttpPut("{ticketId}/addAttachment", Name = "AttachToTicket")]
        public void AttachToTicket(int ticketId, [FromBody] TicketAttachment attachment)
        {
            _ticketService.AddAttachmentToTicketAsync(ticketId, attachment);
        }
    }
}