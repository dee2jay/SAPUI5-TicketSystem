using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NodaTime;
using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;
using TicketManagementSystem.Infrastructure.Persistence;

namespace TicketManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController(ITicketService ticketService, ITicketAttachmentService attachmentService, ITicketCommentService commentService, IUserService userService ,TicketDbContext dbContext, IAppLogger logger) : ControllerBase
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
    public async Task<IActionResult> UpdateTicket(int ticketId, [FromBody] TicketEditDto dto)
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
    [HttpPost("/api/tickets/{ticketId}/addComment", Name = "AddComment")]
    public async Task<IActionResult> AddComment(int ticketId, [FromBody] TicketCommentDto dto)
    {
        try
        {
            var user = await userService.GetCurrentUser();

            var updateDto = new TicketSystemUpdateDto()
            {
                CommentAdded = true,
                UpdatedAt = SystemClock.Instance.GetCurrentInstant(),
                Comment = dto.Text!,
                UpdatedByUserId = user.Id
            };
            dto.CreatedAt = updateDto.UpdatedAt;
            dto.UserId = user.Id;
            
            var comment = await commentService.AddCommentsByTicketId(ticketId, dto, _cancellationToken.Token);
            await ticketService.UpdateTicketAsync(ticketId, updateDto, _cancellationToken.Token);

            return Ok(comment);
        }
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, e.Source, e.StackTrace!);
            return BadRequest();
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

            IFormFile? file = null;
            foreach (var formFile in Request.Form.Files)
            {
                file = formFile;
                break;
            }

            if (file == null)
            {
                return BadRequest(new { message = "No file supplied." });
            }

            const long maxFileSize = 10 * 1024 * 1024;
            if (file.Length <= 0 || file.Length > maxFileSize)
            {
                return BadRequest(new { message = "File must be between 1 byte and 10 MB." });
            }

            var allowedImageTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            if (!allowedImageTypes.Contains(file.ContentType, StringComparer.OrdinalIgnoreCase))
            {
                return BadRequest(new { message = "Only JPEG, PNG and WebP images are supported." });
            }

            var attachment = await attachmentService.AddAttachment(ticketId, file, user, _cancellationToken.Token);

            var updateDto = new TicketSystemUpdateDto()
            {
                AttachmentAdded = true,
                UpdatedAt = SystemClock.Instance.GetCurrentInstant(),
                UpdatedByUserId = user.Id
            };

            await ticketService.UpdateTicketAsync(ticketId, updateDto, _cancellationToken.Token);
            
            return Ok(MapToDto(attachment));
        }
        catch (Exception e)
        {
            await logger.LogError( e.Message, e,e.Source, e.StackTrace!);
            return StatusCode(500, new { message = e.Message });
        }
        
    }

    private static TicketAttachmentDto? MapToDto(TicketAttachment? attachment)
    {
        if (attachment != null)
        {
            return new TicketAttachmentDto
            {
                FileName = attachment.FileName,
                TicketId = attachment.TicketId,
                UploadAt = attachment.UploadedAt!.Value,
                Url = attachment.Url,
                UserId = attachment.UserId
            };
        }

        return null;
    }

    [Authorize]
    [HttpDelete("/api/tickets/{ticketId}/deleteAttachment/{attachmentId}", Name = "DeleteAttachment")]
    public async Task<IActionResult> DeleteAttachment(int ticketId, string attachmentId)
    {
        try
        {
            var user = await userService.GetCurrentUser();
            var attachment = await attachmentService.GetAttachmentById(attachmentId, _cancellationToken.Token);
            var updateDto = new TicketSystemUpdateDto()
            {
                FileName = attachment.FileName,
                AttachmentDeleted = true,
                UpdatedAt = SystemClock.Instance.GetCurrentInstant(),
                UpdatedByUserId = user.Id
            };
            
            await ticketService.RemoveAttachmentByTicketId(ticketId, attachmentId, _cancellationToken.Token);

            await ticketService.UpdateTicketAsync(ticketId, updateDto, _cancellationToken.Token);
            
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