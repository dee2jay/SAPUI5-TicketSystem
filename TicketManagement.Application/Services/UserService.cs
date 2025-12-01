using Microsoft.AspNetCore.Http;
using System.Net.Http;
using TicketManagementSystem.Application.Command.UserCommands;
using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;


namespace TicketManagementSystem.Application.Services;

public class UserService : IUserService
{
    private readonly ICommandHandler<RegisterUserCommand, User> _registerHandler;
    private readonly ICommandHandler<LoginUserCommand, string> _loginHandler;
    private readonly ICommandHandlerBase<LogoutUserCommand> _logoutHandler;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IUserRepository _userRepository;

    public UserService(ICommandHandler<RegisterUserCommand, User> registerHandler, 
        ICommandHandler<LoginUserCommand, string> loginHandler, 
        ICommandHandlerBase<LogoutUserCommand> logoutHandler, IHttpContextAccessor httpContext, IUserRepository userRepository)
    {
        _registerHandler = registerHandler;
        _loginHandler = loginHandler;
        _logoutHandler = logoutHandler;
        _httpContext = httpContext;
        _userRepository = userRepository;
    }

    public Task<User> RegisterUserAsync(UserDto dto, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        return _registerHandler.Handle(new RegisterUserCommand(dto.Vorname, dto.Name, dto.Username, dto.Email, dto.Password),
            ct);
    }

    public Task<string> LoginUserAsync(LoginUserDto dto, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        return _loginHandler.Handle(new LoginUserCommand(dto.Email, dto.Password), ct);
    }

    public Task LogoutUserAsync(string email)
        => _logoutHandler.Handle(new LogoutUserCommand(email), CancellationToken.None);

    public async Task<User> GetCurrentUser()
    {
        var username = _httpContext.HttpContext?.User?.Identity?.Name;
        var user = await _userRepository.GetUserByUsername(username!);
        return !user.IsError ? user.Value : null!;
    }

    public Task<User?> GetUserById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetUserFullnameById(int userId)
    {
        throw new NotImplementedException();
    }

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }
}
