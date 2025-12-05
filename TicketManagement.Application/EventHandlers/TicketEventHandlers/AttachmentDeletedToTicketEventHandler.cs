using Microsoft.Extensions.DependencyInjection;
using TicketManagementSystem.Application.Events.TicketEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;
using TicketManagementSystem.Infrastructure.Persistence;

namespace TicketManagementSystem.Application.EventHandlers.TicketEventHandlers;

public class AttachmentDeletedToTicketEventHandler(IAppLogger logger, IServiceProvider serviceProvider) : IEventHandler<AttachmentDeletedToTicketEvent>
{
    public async Task HandleAsync(AttachmentDeletedToTicketEvent @event, CancellationToken ct)
    {
        try
        {
            var logEntry = new TicketChangeLog
            {
                Title = "Attachment removed to Ticket",
                TicketId = @event.TicketId,
                Property = "Attachments",
                OldValue = @event.AttachmentId,
                NewValue = null,
                ChangedAt = @event.OccuredOn,
                ChangedBy = @event.User
            };
            var message =
                $"TimeStamp -> {logEntry.ChangedAt}, {logEntry.Title}, TicketId -> {logEntry.TicketId}, Attachment->{logEntry.OldValue}, User -> {logEntry.ChangedBy}";

            //History
            var dbContext = serviceProvider.GetRequiredService<TicketDbContext>();
            dbContext.TicketHistories.Add(
                new History
                {
                    TicketId = @event.TicketId,
                    Action = message,
                    Timestamp = @event.OccuredOn
                });
            await logger.LogInfo(message);
        }
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, nameof(AttachmentAddedToTicketEventHandler), e.StackTrace!);
        }

    }
}