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
    public class AttachmentAddedToTicketEventHandler(IAppLogger logger, IServiceProvider serviceProvider) : IEventHandler<AttachmentAddedToTicketEvent>
    {
        public async Task HandleAsync(AttachmentAddedToTicketEvent @event)
        {
            try
            {
                var logEntry = new TicketChangeLog
                {
                    Title = "New Attachment added to Ticket",
                    TicketId = @event.TicketId,
                    Property = "Attachments",
                    Value = @event.AttachmentName,
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
                
                //send Mail
                //log send mail
            }
            catch (Exception e)
            {
                await logger.LogError(e.Message, e, nameof(AttachmentAddedToTicketEventHandler), e.StackTrace!);
            }
           
        }
    }
}
