namespace TicketManagementSystem.Application.Interfaces;

public interface ITicketAssignmentService
{
    string? GetAssigneeForCategory(string category);
}