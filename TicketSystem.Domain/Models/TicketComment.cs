using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NodaTime;

namespace TicketManagementSystem.Domain.Models;

public class TicketComment
{
    [Key]
    public Guid Id { get; set; }
    public string? Author { get; set; }
    public string? Text { get; set; }
    public Instant? CreatedAt { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    [JsonIgnore] public int TicketId { get; set; }
    [JsonIgnore] public Ticket? Ticket { get; set; }

}