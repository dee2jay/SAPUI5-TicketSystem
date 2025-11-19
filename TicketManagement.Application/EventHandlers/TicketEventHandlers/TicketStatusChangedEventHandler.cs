using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Application.Events.TicketEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Enums;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Application.EventHandlers.TicketEventHandlers;

public class TicketStatusChangedEventHandler : IEventHandler<TicketStatusChangedEvent>
{
    private readonly IAppLogger _logger;

    public TicketStatusChangedEventHandler(IAppLogger logger)
    {
        _logger = logger;
    }

    public async Task HandleAsync(TicketStatusChangedEvent @event)
    {
        var title = @event.NewStatus switch
        {
            TicketStatus.Open => "Ticket Opened",
            TicketStatus.InProgress => "Ticket in Progress",
            TicketStatus.Closed => "Ticket Closed",
            TicketStatus.Standby => "Ticket in Standby",
            _ =>"Unknown Status"
        };
            
            
        var logEntry = new TicketChangeLog
        {
            Title = title,
            TicketId = @event.Ticket.Id,
            Property = nameof(@event.Ticket.Status),
            Value = nameof(@event.NewStatus),
            ChangedAt = DateTime.Now,
            ChangedBy = @event.User
        };
        var message =
            $"TimeStamp -> {logEntry.ChangedAt}, {logEntry.Title}, TicketId -> {logEntry.TicketId}, " +
            $"Value -> {logEntry.Value}, Changed by -> {logEntry.ChangedBy}";

        await _logger.LogInfo(message);
        //send Mail
        //log send mail
    }
}