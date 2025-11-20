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
    public class CommentAddedTicketEventHandler(IAppLogger logger, IServiceProvider serviceProvider) : IEventHandler<CommentAddedToTicketEvent>
    {
        public async Task HandleAsync(CommentAddedToTicketEvent @event)
        {
            try
            {
                var logEntry = new TicketChangeLog
                {
                    Title = "New Comment added to Ticket",
                    TicketId = @event.TicketId,
                    Property = "Comments",
                    Value = @event.Text,
                    ChangedAt = @event.OccuredOn,
                    ChangedBy = @event.User
                };
                var message =
                    $"TimeStamp -> {logEntry.ChangedAt}, {logEntry.Title}, TicketId -> {logEntry.TicketId},  User -> {logEntry.ChangedBy}";

                await logger.LogInfo(message);
                
                //History
                var dbContext = serviceProvider.GetRequiredService<TicketDbContext>();
                dbContext.TicketHistories.Add(
                    new History
                    {
                        TicketId = @event.TicketId,
                        Action = message,
                        Timestamp = @event.OccuredOn
                    });
            
            }
            catch (Exception e)
            {
                await logger.LogError(e.Message, e, nameof(CommentAddedTicketEventHandler), e.StackTrace!);
            }
            
            //send Mail
            //log send mail
        }
    }
}
