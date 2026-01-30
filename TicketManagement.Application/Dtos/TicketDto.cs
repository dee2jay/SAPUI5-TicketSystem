namespace TicketManagementSystem.Application.Dtos;

public class TicketDto
{
    public string? Category { get; set; }
    public string? Location { get; set; }
    public string? CostCenter { get; set; }
    public string? OrderNumber { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
}