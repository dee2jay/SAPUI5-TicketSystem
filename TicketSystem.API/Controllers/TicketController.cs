using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;
using TicketManagement.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;
using TicketSystem.Domain.Models;

namespace TicketManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController(ITicketService ticketService, IAppLogger mongoLogger, IUserService userService) : ControllerBase
    {
        [HttpGet(Name = "GetTickets")]
        public async Task<IActionResult?> GetTickets()
        {
            try
            {
                var tickets = await ticketService.GetAllTicketsAsync();
                return Ok(tickets);
            }
            catch (TimeoutException ex)
            {
                return StatusCode(408, $"Timeout occurred: {ex.Message}");
            }
            catch (Exception e)
            {
                await mongoLogger.LogError(e.Message, e, nameof(ITicketService), e.StackTrace!);
            }

            return null;
        }

        [HttpPost(Name = "AddTicket")]
        public async Task<IActionResult> AddTicket([FromBody] Ticket ticket, string user)
        {
            try
            {
                ticket.Author = user;
                await ticketService.CreateTicketAsync(ticket, user);
                return CreatedAtAction(nameof(AddTicket), new { id = ticket.Id }, ticket);
            }
            catch (TimeoutException ex)
            {
                return StatusCode(408, $"Timeout occurred: {ex.Message}");
            }
            catch (Exception e)
            {
                await mongoLogger.LogError(e.Message, e, nameof(ITicketService), e.StackTrace!);
            }

            return StatusCode(500, "Internal issue is occured");
        }

        [HttpPut("{ticketId}/assign/{userId}", Name = "AssignTicket")]
        public async Task<IActionResult> AssignTicket(int ticketId, string userId)
        {
            try
            {
                var user = await userService.GetUserById(Convert.ToInt32(userId));
                await ticketService.AssignTicketToUserAsync(ticketId, userId);
                return CreatedAtAction(nameof(AssignTicket), new { id = ticketId },  user);
            }
            catch (TimeoutException ex)
            {
                mongoLogger.LogError($"{StatusCode(408, "Timeout occured")}", ex, nameof(AssignTicket), ex.StackTrace!);
                return StatusCode(408, $"Timeout occurred: {ex.Message}");
            }
            catch (Exception e)
            {
                mongoLogger.LogError(e.Message, e, nameof(AssignTicket), e.StackTrace!);
            }

            return StatusCode(500, "Internal issue is occured");
        }

        [HttpPut("{ticketId}/close", Name = "CloseTicket")]
        public void CloseTicket(int ticketId)
        {
            ticketService.CloseTicketAsync(ticketId);
        }

        [HttpPut("{ticketId}/addComment", Name = "CommentTicket")]
        public void CommentTicket(int ticketId, [FromBody] TicketComment comment)
        {
             ticketService.AddCommentToTicketAsync(ticketId, comment);
        }

        [HttpPut("{ticketId}/addAttachment", Name = "AttachToTicket")]
        public void AttachToTicket(int ticketId, [FromBody] TicketAttachment attachment)
        {
            ticketService.AddAttachmentToTicketAsync(ticketId, attachment);
        }
    }
}