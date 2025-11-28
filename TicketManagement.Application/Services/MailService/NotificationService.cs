using System.Net.Sockets;
using TicketManagementSystem.Application.Events.TicketEvents;
using TicketManagementSystem.Application.Events.UserEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Enums;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;
using TicketManagementSystem.Infrastructure.Migrations;

namespace TicketManagementSystem.Application.Services.MailService;

public class NotificationService(ISmtpSettingsProvider emailProvider, IAppLogger logger) : INotificationService
{
    public Task NotifyTicketUpdatedAsync(Ticket ticket)
    {
        throw new NotImplementedException();
    }

    public async Task NotifyAttachmentAddedAsync(Ticket ticket)
    {
        var recipients = new List<string>() { ticket.Author };
        var emailContent = new EmailContent()
        {
            To = recipients,
            Subject = $"Update ticket: {ticket.Title}",
            Body = $"<p>A new Attachment has been added to the ticket <strong>{ticket.Title}</strong>.</p>"
        };

        try
        {
            await emailProvider.GenerateAndSendEmail(emailContent, CancellationToken.None);
            await logger.LogInfo("Email sent!!!", $"{nameof(NotificationService)} -> {nameof(NotifyAttachmentAddedAsync)}");
        }
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, e.Source, e.StackTrace!);
        }
    }

    public async Task NotifyCommentAddedAsync(Ticket ticket)
    {
        var recipients = new List<string>() { ticket.Author };
        var emailContent = new EmailContent()
        {
            To = recipients,
            Subject = $"Update ticket: {ticket.Title}",
            Body = $"<p>A new Comment has been added to the ticket <strong>{ticket.Title}</strong>.</p>"
        };

        try
        {
            await emailProvider.GenerateAndSendEmail(emailContent, CancellationToken.None);
            await logger.LogInfo("Email sent!!!", $"{nameof(NotificationService)} -> {nameof(NotifyCommentAddedAsync)}");
        }
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, e.Source, e.StackTrace!);
        }
    }

    public async Task NotifyPriorityChangedAsync(Ticket ticket)
    {
        var recipients = new List<string>() { ticket.Author };
        var emailContent = new EmailContent()
        {
            To = recipients,
            Subject = $"Update ticket: {ticket.Title}",
            Body = $"<p>The priority of the ticket <strong>{ticket.Title}</strong>. has been changed to {ticket.Priority}</p>"
        };

        try
        {
            await emailProvider.GenerateAndSendEmail(emailContent, CancellationToken.None);
            await logger.LogInfo("Email sent!!!", $"{nameof(NotificationService)} -> {nameof(NotifyPriorityChangedAsync)}");
        }
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, e.Source, e.StackTrace!);
        }
    }

    public async Task NotifyStatusChangedAsync(TicketStatusChangedEvent evt, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        var recipients = new List<string>() { evt.AssignedTo };
        var emailContent = new EmailContent()
        {
            To = recipients,
            Subject = $"Update ticket: {evt.Ticket.Title}",
            Body = $"<p>The status of the ticket <strong>{evt.Ticket.Title}</strong>. has been changed from Status " +
                   $"<strong>{evt.OldStatus}</strong> to <strong>{evt.NewStatus}</strong> by {evt.ChangeBy}</p>"
        };

        try
        {
            if (evt.Ticket.Status == TicketStatus.Closed)
            {
                //await emailProvider.GenerateAndSendEmail(emailContent, CancellationToken.None);
                await logger.LogInfo("Email sent!!!", $"{nameof(NotificationService)} -> {nameof(NotifyStatusChangedAsync)}");
            }
            
        }
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, e.Source, e.StackTrace!);
        }
    }

    public async Task NotifyTicketOwnerChanged(TicketOwnerChangedEvent evt, CancellationToken ct)
    {
        var recipients = new List<string>() { evt.Ticket.Author, evt.OldAssignedUser, evt.NewAssignedUser };
        var emailContent = new EmailContent()
        {
            To = recipients,
            Subject = $"Update ticket: {evt.Ticket.Id}",
            Body = $"<p>The ticket <strong>{evt.Ticket.Title}</strong>. has been assigned from {evt.OldAssignedUser}to {evt.NewAssignedUser}</p>"
        };

        try
        {
            await emailProvider.GenerateAndSendEmail(emailContent, ct);
            await logger.LogInfo("Email sent!!!", $"{nameof(NotificationService)} -> {nameof(NotifyTicketOwnerChanged)}");
        }
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, e.Source, e.StackTrace!);
        }
    }

    public async Task NotifyDueDateAdded(DueDateAddedToTicketEvent @event, CancellationToken ct)
    {
        var recipients = new List<string>() { @event.Ticket.Author, @event.Ticket.AssignedTo! };
        var emailContent = new EmailContent() {
            To = recipients,
            Subject = $"Due date added to ticket: {@event.Ticket.Title}",
            Body = $"<p>A due date of <strong>{@event.DueDate}</strong> has been added to the ticket <strong>{@event.Ticket.Title}</strong> by {@event.ChangeBy}.</p>"
            };
        try
        {
            await emailProvider.GenerateAndSendEmail(emailContent, ct);
            await logger.LogInfo("Email sent!!!", $"{nameof(NotificationService)} -> {nameof(NotifyDueDateAdded)}");
        }
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, e.Source, e.StackTrace!);
        }
    }

    public async Task NotifyTicketCreatedAsync(TicketCreatedEvent evt, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        var subject = $"[New Ticket #{evt.TicketId}] {evt.Title}";
        var body = $@"A new ticket has been created. 
                    Ticket ID: {evt.TicketId}
                    Title: {evt.Title}
                    Created By: {evt.CreatedBy}

                    You can view the ticket in the Ticket Management System.
                    ";

        var emailContent = new EmailContent
        {
            To = [evt.CreatedBy, evt.AssignedTo],
            Subject = subject,
            Body = body
        };

        try
        {
            //await emailProvider.GenerateAndSendEmail(emailContent, CancellationToken.None);
            await logger.LogInfo("Email sent!!!", $"{nameof(NotificationService)} -> {nameof(NotifyTicketCreatedAsync)}");
        }
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, e.Source, e.StackTrace!);
        }
    }

    public async Task NotifyUserCreatedAsync(UserCreatedEvent evt, CancellationToken ct)
    {
        List<string> recipients = [evt.Email];
        var emailContent = new EmailContent()
        {
            To = recipients,
            Subject = $"Welcome to the Ticket system ",
            Body = $"<p>Your account has been created <ul>{evt.Email}</ul></p>"
        };
        try
        {
            await emailProvider.GenerateAndSendEmail(emailContent, CancellationToken.None);
            await logger.LogInfo("Email sent!!!", $"{nameof(NotificationService)} -> {nameof(NotifyUserCreatedAsync)}");
        }
        catch (Exception e)
        {
            await logger.LogError(e.Message, e, e.Source, e.StackTrace!);
        }
        
    }

    ValueTask IAsyncDisposable.DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }
}