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

namespace TicketManagementSystem.Application.EventHandlers.TicketEventHandlers;

public class TicketPriorityChangedEventHandler(IAppLogger logger, IServiceProvider serviceProvider) : IEventHandler<TicketPriorityChangedEvent>
{
    public async Task HandleAsync(TicketPriorityChangedEvent @event)
    {
        try
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
            //log send mail
        }
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, nameof(TicketPriorityChangedEventHandler), e.StackTrace!);
        }
        
    }
}