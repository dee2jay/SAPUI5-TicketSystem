using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController(ITicketService ticketService) : ControllerBase
{
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    //[Authorize]
    [HttpGet(Name = "GetTickets")]
    public async Task<IActionResult?> GetTickets(CancellationToken cancellationToken = default)
    {
        try
        {
            var tickets = await ticketService.GetAllTicketsAsync(cancellationToken);
            return Ok(new { Tickets = tickets });
        }
        catch (TimeoutException ex)
        {
            return StatusCode(408, $"Timeout occurred: {ex.Message}");
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = e.Message });
        }
    }
    //[Authorize]
    [HttpGet("{ticketId}", Name = "GetTicket")]
    public async Task<IActionResult> GetTicket(int ticketId)
    {
        try
        {
            var ticket = await ticketService.GetTicketById(ticketId, _cancellationToken);
            return Ok(new
            {
                Ticket = ticket
            });
        }
        catch (TimeoutException ex)
        {
            return StatusCode(408, new { message = ex.Message });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = e.Message });
        }

    }

    //[Authorize]
    [HttpPost("/create",Name = "CreateTicket")]
    public async Task<IActionResult> AddTicket([FromBody] TicketDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ticket = await ticketService.CreateTicketAsync(dto, _cancellationToken);
            return Ok(new { message = "Ticket created successfully!", Ticket = ticket });
        }
        catch (TimeoutException ex)
        {
            return StatusCode(408, $"Timeout occurred: {ex.Message}");
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = e.Message });
        }
    }

    [Authorize]
    [HttpPut("{ticketId}", Name = "UpdateTicket")]
    public async Task<IActionResult> UpdateTicket(int ticketId, [FromBody] TicketUpdateDto dto, CancellationToken ct)
    {
        try
        {
            await ticketService.UpdateTicketAsync(ticketId, dto, ct);
            return Ok(new { maessage = "Ticket changed successfully!" });
        }
        catch (Exception e)
        {
            return BadRequest(new{message=e.Message});
        }
    }
    
}