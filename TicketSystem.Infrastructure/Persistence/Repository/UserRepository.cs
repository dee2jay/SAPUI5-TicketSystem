using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
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

    public async Task AddUser(User user)
    {
        try
        {
            await _dbcontext.Users.AddAsync(user);
            await _dbcontext.SaveChangesAsync();
        }
        catch (DbException e)
        {
            _logger.LogError(e.Message);
        }
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

    public async Task<ErrorOr<User>> GetUserByUsername(string username)
    {
        var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user != null)
        {
            return user;
        }
        _logger.LogWarning($"User with the username {username} not found.");
        return Error.NotFound(description: $"User with the username {username} not found.");
    }
    public async Task<ErrorOr<User>> GetUserByEmail(string email)
    {
        var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user != null)
        {
            return user;
        }
        _logger.LogWarning($"User with the Email {email} not found.");
        return Error.NotFound(description: $"User with the Email {email} not found.");
    }

    public async Task UpdateUser(User user)
    {
        _dbcontext.Users.Update(user);
        await _dbcontext.SaveChangesAsync();
        _logger.LogInformation($"User with Email {user.Email} has been updated.");
    }

    public async Task RemoveUser(int userId)
    {
        var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user != null)
        {
            var test = await CloseConnectionByUser(user);

            if (test)
            {
                _dbcontext.Users.Remove(user);
            }
            _logger.LogInformation($"User with ID {userId} has been deleted.");
        }
        _logger.LogWarning($"User with ID {userId} not found.");
    }

    private Task<bool> CloseConnectionByUser(User user)
    {
        if (user.UserConnected)
        {
            user.UserConnected = false;
        }
        return Task.FromResult(true);
    }
}