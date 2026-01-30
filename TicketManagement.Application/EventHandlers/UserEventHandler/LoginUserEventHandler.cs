using TicketManagementSystem.Application.Events.UserEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Application.EventHandlers.UserEventHandler;

public class LoginUserEventHandler(IAppLogger logger) : IEventHandler<UserLoginEvent>
{
    public async Task HandleAsync(UserLoginEvent @event, CancellationToken ct)
    {
        var logEntry = new UserChangeLog
        {
            Title = "User logged in",
            Property = "Login",
            OldValue = null,
            NewValue = "True",
            ChangedAt = @event.OccuredOn,
        };
        var message =
            $"TimeStamp-> {logEntry.ChangedAt}, {logEntry.Title}, Email -> {logEntry.NewValue}, " +
            $"Login Status {logEntry.NewValue}";
            
        await logger.LogInfo(message);
    }
}