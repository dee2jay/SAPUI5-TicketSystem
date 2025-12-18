using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NodaTime;

namespace TicketManagementSystem.Domain.Models;

public class TicketAttachment
{
    [Key] public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public byte[] Data { get; set; } = [];
    public Instant? UploadedAt { get; set; }
    public int UserId { get; set; }
    public int TicketId { get; set; }
}