using NodaTime;

namespace TicketManagementSystem.Domain.Models;

public class History
{
    public int Id { get; set; }
    public string? Action { get; set; }
    public Instant? Timestamp { get; set; }
    public int TicketId { get; set; }
        
}