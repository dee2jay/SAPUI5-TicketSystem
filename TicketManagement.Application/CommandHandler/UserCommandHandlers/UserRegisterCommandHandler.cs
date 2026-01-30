using AutoMapper;
using NodaTime;
using TicketManagementSystem.Application.Command.UserCommands;
using TicketManagementSystem.Application.Events.UserEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Application.Results;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Application.CommandHandler.UserCommandHandlers;

public class UserRegisterCommandHandler(
    IUserRepository userRepo,
    IMapper mapper,
    IEventPublisher eventPublisher,
    IAppLogger logger)
    : ICommandHandler<RegisterUserCommand, RegisterUserResult>
{
    private readonly IMapper _mapper = mapper;

    public async Task<RegisterUserResult> Handle(RegisterUserCommand command, CancellationToken ct)
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
            if (userRepo.GetUserByEmail(command.Email).Result.Value != null)
            {
                return RegisterUserResult.Fail("EMAIL_ALREADY_USED", 
                    "An account already exists with this email address.");
            }
            
            await userRepo.AddUser(user);
            await eventPublisher.PublishEventAsync(userRegisterEvent,ct );
            return RegisterUserResult.Ok(user.Id);
        }
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, e.Source, e.StackTrace!);
            return null!;
        }
            
    }

    Task ICommandHandlerBase<RegisterUserCommand>.Handle(RegisterUserCommand command, CancellationToken ct)
    {
        return Handle(command, ct);
    }
}