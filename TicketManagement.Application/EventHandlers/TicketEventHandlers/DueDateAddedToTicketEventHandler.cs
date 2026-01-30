using Microsoft.Extensions.DependencyInjection;
using TicketManagementSystem.Application.Events.TicketEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;
using TicketManagementSystem.Infrastructure.Persistence;

namespace TicketManagementSystem.Application.EventHandlers.TicketEventHandlers;

public class DueDateAddedToTicketEventHandler : IEventHandler<DueDateAddedToTicketEvent>
{
    private readonly IAppLogger _logger;
    private readonly IServiceProvider serviceProvider;
    private readonly INotificationService notificationService;

    public DueDateAddedToTicketEventHandler(IAppLogger logger, IServiceProvider serviceProvider, INotificationService notificationService)
    {
        _logger = logger;
        this.serviceProvider = serviceProvider;
        this.notificationService = notificationService;
    }

    public async  Task HandleAsync(DueDateAddedToTicketEvent @event, CancellationToken ct)
    {
        var logEntry = new TicketChangeLog
        {
            Title = "New due date",
            TicketId = @event.Ticket.Id,
            Property = nameof(@event.Ticket.DueDate),
            OldValue = @event.Ticket.DueDate.ToString(),
            NewValue = @event.DueDate,
            ChangedAt = @event.OccuredOn,
            ChangedBy = @event.ChangeBy
        };
        var message =
            $"TimeStamp -> {logEntry.ChangedAt}, {logEntry.Title}, TicketId -> {logEntry.TicketId}, " +
            $"Property -> {logEntry.Property}, OldValue -> {logEntry.OldValue}, NewValue -> {logEntry.NewValue}, " +
            $"Changed by -> {logEntry.ChangedBy}";

        await _logger.LogInfo(message);

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
        await notificationService.NotifyDueDateAdded(@event, ct);
    }
}