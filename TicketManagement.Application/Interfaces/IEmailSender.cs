using TicketManagementSystem.Application.Services.Mail;

namespace TicketManagementSystem.Application.Interfaces;

public interface IEmailSender
{
    Task SendEmailAsync(EmailContent email, CancellationToken cancellationToken = default);
}