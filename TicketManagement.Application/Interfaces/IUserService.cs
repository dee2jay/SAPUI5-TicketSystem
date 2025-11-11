using TicketManagementSystem.Domain.Models;

namespace TicketManagement.Application.Interfaces
{
    public interface IUserService
    {
        public Task<bool> UserExistsAsync(string userId);
        public Task<User> GetUserById(int userId);
    }
}
