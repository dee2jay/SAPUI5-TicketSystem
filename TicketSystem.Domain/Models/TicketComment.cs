using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Domain.Models;

public class TicketComment
{
    [Key]
    public Guid Id { get; set; }
    public int TicketId { get; set; }
    public string? Author { get; set; }
    public string? Text { get; set; }
    public DateTime CreatedAt { get; set; }
}