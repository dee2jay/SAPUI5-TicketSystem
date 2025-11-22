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

public class RegisterUserEventHandler : IEventHandler<UserCreatedEvent>
{
    private readonly IAppLogger _logger;
    private readonly INotificationService _notificationService;

    public RegisterUserEventHandler(IAppLogger logger, INotificationService notificationService)
    {
        _logger = logger;
        _notificationService = notificationService;
    }

    public async Task HandleAsync(UserCreatedEvent @event, CancellationToken ct)
    {
        var logEntry = new UserChangeLog
        {
            Title = "New User Created",
            Property = nameof(@event.Email),
            OldValue = null,
            NewValue = @event.Email,
            ChangedAt = @event.Timestamp,
        };
        var message =
            $" TimeStamp -> {logEntry.ChangedAt},{logEntry.Title}: Email -> {logEntry.NewValue}";

        await _logger.LogInfo(message);
        
        //send Mail
        await _notificationService.NotifyUserCreatedAsync(@event, ct);
        
        //log send mail
        await _logger.LogInfo($"Email sent to {@event.Email}", nameof(RegisterUserEventHandler));
    }
}