using NodaTime;

namespace TicketManagementSystem.Application.Dtos;

public class TicketSystemUpdateDto
{
    public bool AttachmentAdded { get; set; } = false;
    public bool AttachmentDeleted { get; set; } = false;
    public string Comment { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public bool CommentAdded { get; set; } = false;
    public Instant UpdatedAt { get; set; }
    public int UpdatedByUserId { get; set; }
}