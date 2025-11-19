using Microsoft.Extensions.DependencyInjection;
using TicketManagementSystem.Application.Events.TicketEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;
using TicketManagementSystem.Infrastructure.Logging;
using TicketManagementSystem.Infrastructure.Persistence;

namespace TicketManagementSystem.Application.EventHandlers.TicketEventHandlers;

public class TicketCreatedEventHandler : IEventHandler<TicketCreatedEvent>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IAppLogger _logger;
    public TicketCreatedEventHandler(IServiceProvider serviceProvider, IAppLogger logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task HandleAsync(TicketCreatedEvent @event)
    {
        try
        {
            var logEntry = new TicketChangeLog
            {
                Title = "New Ticket Created",
                TicketId = @event.TicketId,
                Property = "Title",
                Value = @event.Value,
                ChangedAt = @event.OccuredOn,
                ChangedBy = @event.ChangedBy
            };
            var message =
                $"Timestamp -> {logEntry.ChangedAt}, {logEntry.Title}, " +
                $"TicketId -> {logEntry.TicketId}, User -> {logEntry.ChangedBy}";

            await using var logger = _serviceProvider.GetRequiredService<IAppLogger>();
            await logger.LogInfo(message);

            //History

            await using var dbContext = _serviceProvider.GetRequiredService<TicketDbContext>();
            dbContext.TicketHistories.Add(
                new History
                {
                    TicketId = @event.TicketId,
                    Action = message,
                    Timestamp = @event.OccuredOn
                });
            await dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            await _logger.LogError(e.Message, e, nameof(TicketCreatedEventHandler), e.StackTrace!);
        }
        
        //send Mail
        //log send mail
    }
}