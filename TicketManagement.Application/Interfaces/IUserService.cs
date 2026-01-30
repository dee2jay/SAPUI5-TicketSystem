using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Application.Results;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Interfaces;

public interface IUserService : IAsyncDisposable
{
    Task<RegisterUserResult> RegisterUserAsync(UserDto dto, CancellationToken ct);
    Task<LoginResult> LoginUserAsync(LoginUserDto dto, CancellationToken ct);
    Task LogoutUserAsync(string email);
    public Task<User> GetCurrentUser();
    Task<User?> GetUserById(int userId);
    Task<string?> GetUserFullnameById(int  userId);
}