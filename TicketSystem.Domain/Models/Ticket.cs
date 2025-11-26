using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TicketManagementSystem.Domain.Enums;

namespace TicketManagementSystem.Domain.Models;

public class Ticket
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public TicketStatus Status { get; set; }
    public TicketPriority Priority { get; set; }
    public string Category { get; set; } = string.Empty;
    [Required] public string Location { get; set; } = string.Empty;
    public string CostCenter { get; set; } = string.Empty;
    public string? AssignedTo { get; set; }
    public string OrderNumber { get; set; }

    public int? UserId { get; set; }
    public User? User { get; set; }

    public ICollection<TicketAttachment> Attachments { get; set; } = [];
    public ICollection<TicketComment> Comments { get; set; } = [];
    public List<History> Histories { get; set; } = new();

    
    [Timestamp]
    public byte[] RowVersion { get; set; }
    
}