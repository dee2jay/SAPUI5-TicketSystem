using System.ComponentModel.DataAnnotations;
using TicketSystem.Domain.Enums;
using TicketSystem.Domain.Models;

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
    public ICollection<TicketAttachment> Attachments { get; set; } = [];
    public ICollection<TicketComment> Comments { get; set; } = [];

    private Ticket()
    {
        
    }

    public Ticket(string category, string location, string costCenter, string orderNumber, string title, string description)
    {
        Category = category;
        Location = location;
        CostCenter = costCenter;
        OrderNumber = orderNumber;
        Title = title;
        Description = description;
        CreatedAt = DateTime.UtcNow;
    }

    
}