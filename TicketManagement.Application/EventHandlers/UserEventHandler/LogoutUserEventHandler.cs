using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Application.Events.UserEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Application.EventHandlers.UserEventHandler;

public class LogoutUserEventHandler : IEventHandler<UserLogoutEvent>
{
    private readonly IAppLogger _logger;

    public LogoutUserEventHandler(IAppLogger logger)
    {
        _logger = logger;
    }

    public async Task HandleAsync(UserLogoutEvent @event, CancellationToken ct)
    {
        var logEntry = new UserChangeLog
        {
            Title = "User logged out",
            Property = "UserConnected",
            OldValue = "True",
            NewValue = "False",
            ChangedAt = @event.OccuredOn,
        };
        var message =
            $"TimeStamp-> {logEntry.ChangedAt},{logEntry.Title}, Email -> {logEntry.NewValue}, " +
            $"Login status -> {logEntry.NewValue}";
        await _logger.LogInfo(message);
    }
}