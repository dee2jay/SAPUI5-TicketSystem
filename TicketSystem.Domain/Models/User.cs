using System.ComponentModel.DataAnnotations;
using NodaTime;

namespace TicketManagementSystem.Domain.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public required string Email { get; set; }
    public required string Username { get; set; } = string.Empty;
    public required string Password { get; set; } = string.Empty;
    public int TokenVersion { get; set; }
    public Instant? LastRefreshAt { get; set; }
    public List<Ticket> Tickets { get; set; } = [];
    public List<TicketAttachment> Attachments { get; set; } = [];
    public List<TicketComment> Comments { get; set; } = [];
}