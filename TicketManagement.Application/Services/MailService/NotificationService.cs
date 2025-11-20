using System.Net.Sockets;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Services.MailService;

public class NotificationService(ISmtpSettingsProvider emailProvider) : INotificationService
{
    ValueTask IAsyncDisposable.DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
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

        await emailProvider.GenerateAndSendEmail(emailContent, CancellationToken.None);
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

        await emailProvider.GenerateAndSendEmail(emailContent, CancellationToken.None);
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

        await emailProvider.GenerateAndSendEmail(emailContent, CancellationToken.None);
    }

    public async Task NotifyStatusChangedAsync(Ticket ticket)
    {
        var recipients = new List<string>() { ticket.Author };
        var emailContent = new EmailContent()
        {
            To = recipients,
            Subject = $"Update ticket: {ticket.Title}",
            Body = $"<p>The status of the ticket <strong>{ticket.Title}</strong>. has been changed to {ticket.Status}</p>"
        };

        await emailProvider.GenerateAndSendEmail(emailContent, CancellationToken.None);
    }

    public async Task NotifyTicketCreatedAsync(Ticket ticket)
    {
        var recipients = new List<string>() { ticket.Author };
        var emailContent = new EmailContent()
        {
            To = recipients,
            Subject = $"New ticket: {ticket.Title}",
            Body = $"<p>A new ticket  has been created <strong>{ticket.Title}</strong>.</p>"
        };

        await emailProvider.GenerateAndSendEmail(emailContent, CancellationToken.None);
    }

    public async Task NotifyTicketAssignedToUserAsync(Ticket ticket)
    {
        var recipients = new List<string>() { ticket.Author, ticket.AssignedTo };
        var emailContent = new EmailContent()
        {
            To = recipients,
            Subject = $"Update ticket: {ticket.Title}",
            Body = $"<p>The ticket <strong>{ticket.Title}</strong>. has been assigned to {ticket.AssignedTo}</p>"
        };

        await emailProvider.GenerateAndSendEmail(emailContent, CancellationToken.None);
    }

    public async Task NotifyUserCreatedAsync(User user)
    {
        var recipients = new List<string>() { user.Email };
        var emailContent = new EmailContent()
        {
            To = recipients,
            Subject = $"Welcome to the Ticket system ",
            Body = $"<p>Your account has been created <ul>{user.Email}</strong></p>"
        };

        await emailProvider.GenerateAndSendEmail(emailContent, CancellationToken.None);
    }
}