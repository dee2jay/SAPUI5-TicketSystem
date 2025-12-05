using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NodaTime;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Application.Events.TicketEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;
using TicketManagementSystem.Infrastructure.Persistence;

namespace TicketManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController(ITicketService ticketService, IUserService userService ,TicketDbContext dbContext, IAppLogger logger, IEventPublisher eventPublisher) : ControllerBase
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
    [HttpGet("/api/tickets/{ticketId}", Name = "GetTicket")]
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
    [HttpPost("/api/tickets/create",Name = "CreateTicket")]
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
    [HttpGet("/api/tickets/{ticketId}/history/", Name = "GetHistory")]
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
    [HttpPut("/api/tickets/{ticketId}/update", Name = "UpdateTicket")]
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
    [HttpGet("/api/tickets/{ticketId}/histories", Name = "GetHistories")]
    public async Task<IActionResult> GetHistories(int ticketId)
    {
        try
        {
            var histories = await ticketService.GetHistoryByTicketId(ticketId, _cancellationToken.Token);
            return Ok(histories);
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = e.Message });
        }
    }


    [Authorize]
    [HttpGet("/api/tickets/{ticketId}/comments", Name = "GetComments")]
    public async Task<IActionResult> GetComments(int ticketId)
    {
        try
        {
            var comments = await ticketService.GetCommentsByTicketId(ticketId, _cancellationToken.Token);
            return Ok(comments);
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = e.Message });
        }
    }

    [Authorize]
    [HttpGet("/api/tickets/{ticketId}/attachments", Name = "GetAttachment")]
    public async Task<IActionResult> GetAttachments(int ticketId)
    {
        var attachments = await ticketService.GetAttachmentsByTicketId(ticketId, _cancellationToken.Token);
        
        return Ok(attachments);
    }

    [Authorize]
    [HttpPost("/api/tickets/{ticketId}/uploadAttachment", Name = "UploadAttachment")]
    public async Task<IActionResult> UploadAttachment(int ticketId)
    {
        try
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
                UploadedAt = SystemClock.Instance.GetCurrentInstant(),
                UserId = user.Id,
                User = user
            };

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            attachment.Data = ms.ToArray();

            dbContext.TicketAttachments.Add(attachment);

            await eventPublisher.PublishEventAsync(
                new AttachmentAddedToTicketEvent(ticketId, attachment.FileName, $"{user.FirstName}, {user.LastName}"),
                _cancellationToken.Token);
            
            await dbContext.SaveChangesAsync(_cancellationToken.Token);
            
            return Ok(new
            {
                id = attachment.Id,
                filename = attachment.FileName,
                url = $"api/attachment/{attachment.Id}"
            });
        }
        catch (Exception e)
        {
            await logger.LogError( e.Message, e,e.Source, e.StackTrace!);
            return StatusCode(500, new { message = e.Message });
        }
        
    }

    [Authorize]
    [HttpDelete("/api/tickets/{ticketId}/attachment/{attachmentId}", Name = "DeleteAttachment")]
    public async Task<IActionResult> DeleteAttachment(int ticketId, string attachmentId)
    {
        try
        {
            var user = await userService.GetCurrentUser();
            
            await ticketService.RemoveAttachmentByTicketId(ticketId, attachmentId, _cancellationToken.Token);

            await eventPublisher.PublishEventAsync(
                new AttachmentDeletedToTicketEvent(ticketId, attachmentId, $"{user.FirstName},{user.LastName}"),
                _cancellationToken.Token);

            await dbContext.SaveChangesAsync(_cancellationToken.Token);
            
            return Ok(new
            {
                ticketId,
                attachmentId,
                deleted = true
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = e.Message });
        }
    }
}