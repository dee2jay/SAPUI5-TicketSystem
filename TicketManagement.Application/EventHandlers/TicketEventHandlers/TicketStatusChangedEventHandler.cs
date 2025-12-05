using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;
using TicketManagementSystem.Application.Events.TicketEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Enums;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;
using TicketManagementSystem.Infrastructure.Persistence;

namespace TicketManagementSystem.Application.EventHandlers.TicketEventHandlers;

public class TicketStatusChangedEventHandler(IAppLogger logger, IServiceProvider serviceProvider, INotificationService notificationService) : IEventHandler<TicketStatusChangedEvent>
{
    public async Task HandleAsync(TicketStatusChangedEvent @event, CancellationToken ct)
    {
        try
        {
            var title = @event.NewStatus switch
            {
                TicketStatus.Open => "Ticket Opened",
                TicketStatus.InProgress => "Ticket in Progress",
                TicketStatus.Closed => "Ticket Closed",
                TicketStatus.Rejected => "Ticket Rejected",
                TicketStatus.Waiting => "Ticket Waiting",
                _ => "Unknown Status"
            };

            var logEntry = new TicketChangeLog
            {
                Title = title,
                TicketId = @event.Ticket.Id,
                Property = nameof(@event.Ticket.Status),
                OldValue = @event.OldStatus.ToString(),
                NewValue = @event.NewStatus.ToString(),
                ChangedAt = SystemClock.Instance.GetCurrentInstant(),
                ChangedBy = $"{@event.ChangeBy.LastName} {@event.ChangeBy.FirstName}"
            };
            var message =
                $"TimeStamp -> {logEntry.ChangedAt}, {logEntry.Title}, TicketId -> {logEntry.TicketId}, " +
                $"Property -> {logEntry.Property}, OldValue -> {logEntry.OldValue}, " +
                $"NewValue -> {logEntry.NewValue}, Changed by -> {logEntry.ChangedBy}";

            await logger.LogInfo(message);

            //History
            await using var dbContext = serviceProvider.GetRequiredService<TicketDbContext>();
            dbContext.TicketHistories.Add(
                new History
                {
                    TicketId = @event.Ticket.Id,
                    Action = message,
                    Timestamp = @event.OccuredOn
                });
            
            
            await notificationService.NotifyStatusChangedAsync(@event, ct);

            
        }
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, nameof(TicketStatusChangedEventHandler), e.StackTrace!);
        }
       
    }
}