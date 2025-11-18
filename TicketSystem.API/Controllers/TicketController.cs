using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Enums;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;
using TicketSystem.Domain.Models;

namespace TicketManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController(ITicketService ticketService, IAppLogger mongoLogger, IUserService userService) : ControllerBase
{
    [Authorize]
    [HttpGet(Name = "GetTickets")]
    public async Task<IActionResult?> GetTickets(CancellationToken cancellationToken = default)
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

    [Authorize]
    [HttpPost(Name = "AddTicket")]
    public async Task<IActionResult> AddTicket([FromBody] TicketDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ticket = await ticketService.CreateTicketAsync(dto);
            return CreatedAtAction(nameof(AddTicket), new { id = ticket.Id }, ticket.Author);
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
            await mongoLogger.LogError($"{StatusCode(408, "Timeout occured")}", ex, nameof(AssignTicket), ex.StackTrace!);
            return StatusCode(408, $"Timeout occurred: {ex.Message}");
        }
        catch (Exception e)
        {
            await mongoLogger.LogError(e.Message, e, nameof(AssignTicket), e.StackTrace!);
        }

        return StatusCode(500, "Internal issue is occured");
    }

    [HttpPut("/changepriority", Name = "ChangePriority")]
    public async Task<IActionResult> ChangePriority([FromBody] int ticketId, TicketPriority priority)
    {
        try
        {
            var currentTicket = await ticketService.GetTicketById(ticketId);
            if (currentTicket != null)
            {
                var currentPriority = currentTicket?.Priority;
                if (currentPriority == priority)
                {
                    return Empty;
                }

                var newTicket = currentTicket;
                newTicket!.Priority = priority;
                await ticketService.UpdateTicketAsync(ticketId, newTicket);

                return CreatedAtAction(nameof(ChangePriority), new { id = ticketId, 
                    oldPriority = $"{currentPriority.ToString()}", 
                    newPriority = $"{newTicket.Priority.ToString()}" });
            }
            return StatusCode(204, $"No Content for the Ticket with Id{ticketId}");

        }
        catch (Exception e)
        {
            await mongoLogger.LogError(e.Message, e, nameof(TicketsController), e.StackTrace!);
        }

        return Empty;
    }


    [HttpPut("{ticketId}/close", Name = "CloseTicket")]
    public async Task<IActionResult> CloseTicket(int ticketId)
    {
        try
        {
            await ticketService.CloseTicketAsync(ticketId);
            return Ok(new { message = $"Ticket {ticketId} closed successfully." });
        }
        catch (Exception e)
        {
            return StatusCode(500, new{
                error= "Server Internal Issue",
                details= e.Message
                });
        }
    }

    [HttpPut("{ticketId}/addComment", Name = "CommentTicket")]
    public async Task<IActionResult> CommentTicket(int ticketId, [FromBody] TicketComment comment)
    {
        try
        {
            await ticketService.AddCommentToTicketAsync(ticketId, comment);
            return Ok(new { TicketId = ticketId, AddedCommant = comment });
        }
        catch (Exception e)
        {
            mongoLogger.LogError(e.Message, e, nameof(TicketsController), e.StackTrace!);
            return StatusCode(500, new
            {
                error = e.Message,
                Details = e.StackTrace
            });
        }
    }

    [HttpPut("{ticketId}/addAttachment", Name = "AttachToTicket")]
    public void AttachToTicket(int ticketId, [FromBody] TicketAttachment attachment)
    {
        ticketService.AddAttachmentToTicketAsync(ticketId, attachment);
    }
}