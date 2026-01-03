using NodaTime;

namespace TicketManagementSystem.Domain.Models;

public class TicketChangeLog
{
    public string? Title { get; set; }
    public int TicketId{get; set;}
    public string Property { get; set; } = string.Empty;
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public Instant ChangedAt { get; set; }
    public string ChangedBy { get; set; } = string.Empty;
}