using Microsoft.AspNetCore.Mvc;
using TicketManagement.Application.Interfaces;
using TicketManagementSystem.Infrastructure.Interface;
using TicketSystem.Domain.Models;

namespace TicketManagementSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
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

        }
    }
}
