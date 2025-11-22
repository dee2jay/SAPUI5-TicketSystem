using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Application.Events.TicketEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;
using TicketManagementSystem.Infrastructure.Persistence;

namespace TicketManagementSystem.Application.EventHandlers.TicketEventHandlers
{
    public class TicketOwnerChangedEventHandler(IServiceProvider serviceProvider, IAppLogger logger, INotificationService notificationService)
        : IEventHandler<TicketOwnerChangedEvent>
    {
        

        public async Task HandleAsync(TicketOwnerChangedEvent @event, CancellationToken ct)
        {
            try
            {
                var logEntry = new TicketChangeLog
                {
                    Title = "Ticket Owner Changed",
                    TicketId = @event.Ticket.Id,
                    Property = nameof(@event.Ticket.AssignedTo),
                    OldValue = @event.OldAssignedUser,
                    NewValue = @event.NewAssignedUser,
                    ChangedAt = @event.OccuredOn,
                    ChangedBy = @event.ChangeBy
                };
                var message =
                    $"TimeStamp -> {logEntry.ChangedAt}, {logEntry.Title}, TicketId -> {logEntry.TicketId}, " +
                    $"Property -> {logEntry.Property}, OldValue -> {logEntry.OldValue}, NewValue -> {logEntry.NewValue}, " +
                    $"Changed by -> {logEntry.ChangedBy}";

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

                //send Mail
                await notificationService.NotifyTicketOwnerChanged(@event, ct);
                
            }
            catch (Exception e)
            {
                await logger.LogError(e.Message, e, nameof(TicketPriorityChangedEventHandler), e.StackTrace!);
            }
        }
    }
}
