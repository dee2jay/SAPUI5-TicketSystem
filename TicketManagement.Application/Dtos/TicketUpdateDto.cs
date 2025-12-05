using System.Text.Json.Serialization;
using TicketManagementSystem.Domain.Enums;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Dtos;

public class TicketUpdateDto
{
    public string? Title { get; set; }
    public string? Category { get; set; }
    public TicketStatus Status { get; set; }
    public TicketPriority Priority { get; set; }
    public string? AssignedTo { get; set; }

    public List<TicketCommentDto>? Comments { get; set; } = [];
    public List<TicketAttachmentDto>? Attachments { get; set; } = [];
    public List<HistoryDto> Histories { get; set; } = [];
    
    public byte[]? RowVersion { get; set; }
}