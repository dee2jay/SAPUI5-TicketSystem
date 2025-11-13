using TicketManagement.Application.Interfaces;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Services;

public class UserService : IUserService
{
    public Task<bool> UserExistsAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserById(int userId)
    {
        throw new NotImplementedException();
    }

    public string GetCurrentUser()
    {
        return "hardcodedUser";
    }
}