using TicketManagementSystem.Application.Command.UserCommands;
using TicketManagementSystem.Application.Events.UserEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Application.Results;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Application.CommandHandler.UserCommandHandlers;

public class UserLogoutCommandHandler(IUserRepository userRepo, IEventPublisher eventPublisher)
    : ICommandHandler<LogoutUserCommand, LogoutResult>
{
    Task ICommandHandlerBase<LogoutUserCommand>.Handle(LogoutUserCommand command, CancellationToken ct)
    {
        return Handle(command, ct);
    }
    public async Task<LogoutResult> Handle(LogoutUserCommand cmd, CancellationToken ct)
    {
        var user = await userRepo.GetUserByEmail(cmd.Email);
        if (!user.IsError)
        {
            await userRepo.UpdateUser(user.Value);
            await eventPublisher.PublishEventAsync(new UserLogoutEvent(user.Value.Email, user.Value.Username), CancellationToken.None);
            return LogoutResult.Ok();
        }
        return LogoutResult.Fail();
    }
}