using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Application.Events;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Application.EventHandlers;

public class RegisterUserEventHandler : IEventHandler<UserCreatedEvent>
{
    private readonly IAppLogger _logger;

    public RegisterUserEventHandler(IAppLogger logger)
    {
        _logger = logger;
    }

    public async Task HandleAsync(UserCreatedEvent @event)
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
        //log send mail
    }
}