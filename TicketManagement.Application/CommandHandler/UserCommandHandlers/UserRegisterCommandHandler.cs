using AutoMapper;
using NodaTime;
using TicketManagementSystem.Application.Command.UserCommands;
using TicketManagementSystem.Application.Events.UserEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Application.CommandHandler.UserCommandHandlers;

public class UserRegisterCommandHandler : ICommandHandler<RegisterUserCommand, User>
{
    private readonly IUserRepository _userRepo;
    private readonly IMapper _mapper;
    private readonly IEventPublisher _eventPublisher;
    private readonly IAppLogger _logger;
    public UserRegisterCommandHandler(IUserRepository userRepo, IMapper mapper, IEventPublisher eventPublisher, IAppLogger logger)
    {
        _userRepo = userRepo;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    public async Task<User> Handle(RegisterUserCommand command, CancellationToken ct)
    {
        var user = new User
        {
            FirstName = command.Firstname,
            LastName = command.Lastname,
            Username = command.Username,
            Email = command.Email,
            Password = string.Empty
        };
            
        user.Password = BCrypt.Net.BCrypt.HashPassword(command.Password);

        var userRegisterEvent = new UserCreatedEvent(
            command.Firstname,
            command.Lastname,
            command.Email,
            command.Username,
            SystemClock.Instance.GetCurrentInstant());
        try
        {
            ct.ThrowIfCancellationRequested();
            await _userRepo.AddUser(user);
            await _eventPublisher.PublishEventAsync(userRegisterEvent,ct );
            return user;
        }
        catch (Exception e)
        {
            await _logger.LogError(e.Message, e, e.Source, e.StackTrace!);
            return null!;
        }
            
    }

    Task ICommandHandlerBase<RegisterUserCommand>.Handle(RegisterUserCommand command, CancellationToken ct)
    {
        return Handle(command, ct);
    }
}