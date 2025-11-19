using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Application.Events.TicketEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Application.EventHandlers.TicketEventHandlers
{
    public class AttachmentAddedToTicketEventHandler : IEventHandler<AttachmentAddedToTicketEvent>
    {
        private readonly IAppLogger _logger;

        public AttachmentAddedToTicketEventHandler(IAppLogger logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(AttachmentAddedToTicketEvent @event)
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

            await _logger.LogInfo(message);
            //send Mail
            //log send mail
        }
    }
}
