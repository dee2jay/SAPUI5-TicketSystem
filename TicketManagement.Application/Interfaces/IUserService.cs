using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Interfaces;

public interface IUserService
{
    public Task<bool> UserExistsAsync(string userId);
    public Task<User> GetUserById(int userId);
    string GetCurrentUser();
}