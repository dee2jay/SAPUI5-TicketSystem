using AutoMapper;
using ErrorOr;
using System.Net.Sockets;
using TicketManagement.Application.Interfaces;
using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Application.Events;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IAppLogger _logger;
    private readonly IMapper _mapper;
    public UserService(IUserRepository userRepository, IAppLogger logger, IMapper mapper)
    {
        _userRepository = userRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        try
        {
            var existingUserList = await _userRepository.GetAllUsers();
            if (!existingUserList.IsError)
            {
                return existingUserList.Value;
            }
            await _logger.LogWarning("No Users found.", nameof(UserService));
        }
        catch (Exception e)
        {
            await _logger.LogError(e.Message, e, "Database", e.StackTrace!);
        }
       
    }

    public async Task<User> AddUser(UserDto dto)
    {
        var user = _mapper.Map<User>(dto);
        
        await _userRepository.AddUser(user);
        var userCreatedEvent = new UserCreatedEvent
        {
            UserId = user.Id,
            OldValue = null,
            NewValue = user.UserName,
            ChangedAt = DateTime.Now,
            ChangedBy = GetCurrentUser()
        };
        return user;
    }

    public Task<User> UpdateUser(User user)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveUser(User user)
    {
        throw new NotImplementedException();
    }

    public string GetCurrentUser()
    {
        return "hardcodedUser";
    }
}