using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;
using TicketManagementSystem.Infrastructure.Persistence;

namespace TicketManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController(ITicketService ticketService, IUserService userService ,TicketDbContext dbContext) : ControllerBase
{
    private readonly CancellationTokenSource _cancellationToken = new();

    [Authorize]
    [HttpGet(Name = "GetTickets")]
    public async Task<IActionResult?> GetTickets()
    {
        try
        {
            var tickets = await ticketService.GetAllTicketsAsync(_cancellationToken.Token);
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
    [Authorize]
    [HttpGet("/ticket/{ticketId}", Name = "GetTicket")]
    public async Task<IActionResult> GetTicket(int ticketId)
    {
        try
        {
            _cancellationToken.Token.ThrowIfCancellationRequested();
            var ticket = await ticketService.GetTicketById(ticketId, _cancellationToken.Token);
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

    [Authorize]
    [HttpPost("/create",Name = "CreateTicket")]
    public async Task<IActionResult> AddTicket([FromBody] TicketDto dto)
    {
        try
        {
            _cancellationToken.Token.ThrowIfCancellationRequested();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ticketDto = await ticketService.CreateTicketAsync(dto, _cancellationToken.Token);
            return Ok(new { message = "Ticket created successfully!", Ticket = ticketDto });
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
    [HttpGet("/ticket/{ticketId}/history/", Name = "GetHistory")]
    public async Task<IActionResult> GetHistoryFromTicket(int ticketId)
    {
        try
        {
            _cancellationToken.Token.ThrowIfCancellationRequested();
            var historyData = await ticketService.GetHistoryByTicketId(ticketId, _cancellationToken.Token);
            return Ok(new { message = "Request successfully executed!!!", TicketId = ticketId, History = historyData });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = e.Message });
        }
    }

    [Authorize]
    [HttpPut("/ticket/{ticketId}/update", Name = "UpdateTicket")]
    public async Task<IActionResult> UpdateTicket(int ticketId, [FromBody] TicketUpdateDto dto)
    {
        try
        {
            await ticketService.UpdateTicketAsync(ticketId, dto, _cancellationToken.Token);
            return Ok(new { message = "Ticket changed successfully!" });
        }
        catch (Exception e)
        {
            return BadRequest(new{message=e.Message});
        }
    }

    [Authorize]
    [HttpPost("/tickets/{ticketId}/uploadAttachment", Name = "UploadAttachment")]
    public async Task<IActionResult> UploadAttachment(int ticketId)
    {
        var user = await userService.GetCurrentUser();
        var file = Request.Form.Files.FirstOrDefault();
        if (file == null)
        {
            return BadRequest("No file");
        }

        var attachment = new TicketAttachment
        {
            FileName = file.FileName,
            TicketId = ticketId,
            UploadedAt = DateTime.UtcNow,
            UserId = user.Id,
            User = user
        };

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        attachment.Data = ms.ToArray();

        dbContext.TicketAttachments.Add(attachment);
        await dbContext.SaveChangesAsync(_cancellationToken.Token);

        return Ok(new
        {
            id = attachment.Id,
            filename = attachment.FileName,
            url = $"api/attachment/{attachment.Id}"
        });
    }
}