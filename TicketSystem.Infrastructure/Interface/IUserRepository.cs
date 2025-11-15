using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Infrastructure.Interface
{
    public interface IUserRepository
    {
        Task<ErrorOr<IEnumerable<User>>> GetAllUsers();
        Task AddUser(User user);
        Task<ErrorOr<User>> GetUserById(int userId);
        Task UpdateUser(int userId);
        Task RemoveUser(int userId);
    }
}
