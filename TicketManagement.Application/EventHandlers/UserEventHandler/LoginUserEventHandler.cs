using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Application.Events.UserEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Application.EventHandlers.UserEventHandler
{
    public class LoginUserEventHandler : IEventHandler<UserLoginEvent>
    {
        private readonly IAppLogger _logger;

        public LoginUserEventHandler(IAppLogger logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(UserLoginEvent @event, CancellationToken ct)
        {
            var logEntry = new UserChangeLog
            {
                Title = "User logged in",
                Property = "UserConnected",
                OldValue = "False",
                NewValue = "True",
                ChangedAt = @event.OccuredOn,
            };
            var message =
                $"TimeStamp-> {logEntry.ChangedAt}, {logEntry.Title}, Email -> {logEntry.NewValue}, " +
                $"Login Status {logEntry.NewValue}";
            
            await _logger.LogInfo(message);
        }
    }
}
