using Microsoft.Extensions.DependencyInjection;
using TicketManagementSystem.Application.Events.TicketEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;
using TicketManagementSystem.Infrastructure.Persistence;

namespace TicketManagementSystem.Application.EventHandlers.TicketEventHandlers;

public class CommentAddedTicketEventHandler(IAppLogger logger, IServiceProvider serviceProvider) : IEventHandler<CommentAddedToTicketEvent>
{
    public async Task HandleAsync(CommentAddedToTicketEvent @event, CancellationToken ct)
    {
        try
        {
            var logEntry = new TicketChangeLog
            {
                Title = "New Comment added to Ticket",
                TicketId = @event.TicketId,
                Property = "Comments",
                OldValue = null,
                NewValue =  @event.Text,
                ChangedAt = @event.OccuredOn,
                ChangedBy = @event.User
            };
            var message =
                $"TimeStamp -> {logEntry.ChangedAt}, {logEntry.Title}, TicketId -> {logEntry.TicketId},  User -> {logEntry.ChangedBy}";
                
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
            await logger.LogError(e.Message, e, nameof(CommentAddedTicketEventHandler), e.StackTrace!);
        }
    }
}