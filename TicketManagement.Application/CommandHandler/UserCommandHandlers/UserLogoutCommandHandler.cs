using TicketManagementSystem.Application.Command.UserCommands;
using TicketManagementSystem.Application.Events.UserEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Application.CommandHandler.UserCommandHandlers;

public class UserLogoutCommandHandler : ICommandHandlerBase<LogoutUserCommand>
{
    private readonly IUserRepository _userRepo;
    private readonly IEventPublisher _eventPublisher;

    public UserLogoutCommandHandler(IUserRepository userRepo, IEventPublisher eventPublisher)
    {
        _userRepo = userRepo;
        _eventPublisher = eventPublisher;
    }

    public async Task Handle(LogoutUserCommand cmd, CancellationToken ct)
    {
        var user = await _userRepo.GetUserByEmail(cmd.Email);
        if (!user.IsError)
        {
            await _userRepo.UpdateUser(user.Value);
            await _eventPublisher.PublishEventAsync(new UserLogoutEvent(user.Value.Email, user.Value.Username), CancellationToken.None);
        }
    }
}