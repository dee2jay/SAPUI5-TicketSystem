using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Domain.Models;

public class TicketAttachment
{
    [Key]
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public byte[] Data { get; set; } = [];
    public DateTime UploadedAt { get; set; }

    public int UserId { get; set; } 
    public User User { get; set; }


    public int TicketId { get; set; }
    public Ticket Ticket { get; set; }
}