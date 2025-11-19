using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Application.Events.TicketEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Application.EventHandlers.TicketEventHandlers;

public  class TicketPriorityChangedEventHandler: IEventHandler<TicketPriorityChangedEvent>
{
    private readonly IAppLogger _logger;

    public TicketPriorityChangedEventHandler(IAppLogger logger)
    {
        _logger = logger;
    }

    public async Task HandleAsync(TicketPriorityChangedEvent @event)
    {
        var logEntry = new TicketChangeLog
        {
            Title = "Priority Changed",
            TicketId = @event.Ticket.Id,
            Property = nameof(@event.Ticket.Priority),
            Value = nameof(@event.NewPriority),
            ChangedAt = @event.OccuredOn,
            ChangedBy = @event.User
        };
        var message =
            $"TimeStamp -> {logEntry.ChangedAt}, {logEntry.Title}, TicketId -> {logEntry.TicketId}, " +
            $"Changed by -> {logEntry.ChangedBy}";

        await _logger.LogInfo(message);
        //send Mail
        //log send mail
    }
}