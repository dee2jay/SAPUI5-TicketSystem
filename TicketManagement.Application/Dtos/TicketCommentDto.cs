using NodaTime;

namespace TicketManagementSystem.Application.Dtos;

public class TicketCommentDto
{
    public string? Author { get; set; }
    public string? Text { get; set; }
    public Instant CreatedAt { get; set; }
    public int UserId { get; set; }
}