using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Interfaces;

public interface IUserService
{
    public Task<IEnumerable<User>> GetUsersAsync();
    public Task<User> AddUser(UserDto dto);
    public Task<User> UpdateUser(User user);
    public Task<bool> RemoveUser(User user);
    string GetCurrentUser();
    Task<User?> GetUserById(int userId);
}