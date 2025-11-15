using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Infrastructure.Persistence.Repository;

public class UserRepository : IUserRepository
{
    private readonly ILogger<ITicketRepository> _logger;
    private readonly TicketDbContext _dbcontext;

    public UserRepository(ILogger<ITicketRepository> logger, TicketDbContext dbcontext)
    {
        _logger = logger;
        _dbcontext = dbcontext;
    }

    public async Task<ErrorOr<IEnumerable<User>>> GetAllUsers()
    {
        var userList = await _dbcontext.Users.ToListAsync();
        if (userList.Count > 0)
        {
            return userList;
        }

        return Error.NotFound(description: $"No Tickets found");
    }

    public Task AddUser(User user)
    {
        throw new NotImplementedException();
    }

    public async Task<ErrorOr<User>> GetUserById(int userId)
    {
        var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user != null)
        {
            return user;
        }
        _logger.LogWarning($"User with ID {userId} not found.");
        return Error.NotFound(description: $"User with ID {userId} not found.");
    }

    public Task UpdateUser(int userId)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveUser(int userId)
    {
        var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user != null)
        {
            var test = await CloseConnectionByUser(userId);

            if (test)
            {
                _dbcontext.Users.Remove(user);
            }
            _logger.LogInformation($"User with ID {userId} has been deleted.");
        }
        _logger.LogWarning($"User with ID {userId} not found.");
    }

    private async Task<bool> CloseConnectionByUser(int userId)
    {
        throw new NotImplementedException();
    }
}