namespace TicketManagementSystem.Application.Dtos;

public class HistoryDto
{
    public string Action { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}