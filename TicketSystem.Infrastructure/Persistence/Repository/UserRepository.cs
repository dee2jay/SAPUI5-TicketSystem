using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Infrastructure.Persistence.Repository;

public class UserRepository : IUserRepository
{
    private readonly ILogger<ITicketRepository> _logger;
    private readonly TicketDbContext dbContextcontext;

    public UserRepository(ILogger<ITicketRepository> logger, TicketDbContext dbcontext)
    {
        _logger = logger;
        dbContextcontext = dbcontext;
    }

    public async Task<ErrorOr<IEnumerable<User>>> GetAllUsers()
    {
        var userList = await dbContextcontext.Users.ToListAsync();
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
            await dbContextcontext.Users.AddAsync(user);
            await dbContextcontext.SaveChangesAsync();
        }
        catch (DbException e)
        {
            _logger.LogError(e.Message);
        }
    }

    public async Task<ErrorOr<User>> GetUserById(int userId)
    {
        var user = await dbContextcontext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user != null)
        {
            return user;
        }
        _logger.LogWarning($"User with ID {userId} not found.");
        return Error.NotFound(description: $"User with ID {userId} not found.");
    }

    public async Task<ErrorOr<User>> GetUserByUsername(string username)
    {
        var user = await dbContextcontext.Users.FirstOrDefaultAsync(u => u.Email == username);
        if (user != null)
        {
            return user;
        }
        _logger.LogWarning($"User with the username {username} not found.");
        return Error.NotFound(description: $"User with the username {username} not found.");
    }

    public async Task<ErrorOr<User>> GetUserByEmail(string email)
    {
        var user = await dbContextcontext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user != null)
        {
            return user;
        }
        _logger.LogWarning($"User with the Email {email} not found.");
        return Error.NotFound(description: $"User with the Email {email} not found.");
    }

    public async Task UpdateUser(User user)
    {
        dbContextcontext.Users.Update(user);
        await dbContextcontext.SaveChangesAsync();
        _logger.LogInformation($"User with Email {user.Email} has been updated.");
    }

    public async Task RemoveUser(int userId)
    {
        var user = await dbContextcontext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user != null)
        {
            var test = await CloseConnectionByUser(user);

            if (test)
            {
                dbContextcontext.Users.Remove(user);
            }
            _logger.LogInformation($"User with ID {userId} has been deleted.");
        }
        _logger.LogWarning($"User with ID {userId} not found.");
    }

    private Task<bool> CloseConnectionByUser(User user)
    {
        return Task.FromResult(true);
    }
}