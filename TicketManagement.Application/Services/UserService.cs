using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Application.Interfaces;
using TicketManagementSystem.Domain.Models;

namespace TicketManagement.Application.Services
{
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
            throw new NotImplementedException();
        }
    }

}
