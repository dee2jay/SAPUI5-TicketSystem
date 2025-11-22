using Microsoft.Extensions.DependencyInjection;
using TicketManagementSystem.Application.Events.TicketEvents;
using TicketManagementSystem.Application.Events.UserEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Application.Services.MailService;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;
using TicketManagementSystem.Infrastructure.Logging;
using TicketManagementSystem.Infrastructure.Persistence;

namespace TicketManagementSystem.Application.EventHandlers.TicketEventHandlers;

public class TicketCreatedEventHandler : IEventHandler<TicketCreatedEvent>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IAppLogger _logger;
    private readonly INotificationService _notificationService;
    public TicketCreatedEventHandler(IServiceProvider serviceProvider, IAppLogger logger, INotificationService notificationService)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _notificationService = notificationService;
    }

    public async Task HandleAsync(TicketCreatedEvent @event, CancellationToken ct)
    {
        try
        {
            ct.ThrowIfCancellationRequested();
            
            //Logging
            var logEntry = new TicketChangeLog
            {
                Title = "New Ticket Created",
                TicketId = @event.TicketId,
                Property = "Title",
                OldValue = null,
                NewValue = @event.Title,
                ChangedAt = @event.OccuredOn,
                ChangedBy = @event.CreatedBy
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
                    Action = $"Ticket created at {logEntry.ChangedAt} by {logEntry.ChangedBy}",
                    Timestamp = @event.OccuredOn
                });
            await dbContext.SaveChangesAsync(ct);

           await  _notificationService.NotifyTicketCreatedAsync(@event, ct);
        }
        catch (Exception e)
        {
            await _logger.LogError(e.Message, e, nameof(TicketCreatedEventHandler), e.StackTrace!);
        }
        
        
        //log send mail
    }
}