using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Interfaces
{
    public interface IUserService : IAsyncDisposable
    {
        Task<User> RegisterUserAsync(UserDto dto);
        Task<string> LoginUserAsync(LoginUserDto dto);
        Task LogoutUserAsync(string email);
        public Task<User> GetCurrentUser();
        Task<User?> GetUserById(int toInt32);
    }
}
