using TicketManagementSystem.Application.Services.Mail;

namespace TicketManagementSystem.Application.Interfaces;

public interface ISmtpSettingsProvider
{
    Task GenerateAndSendEmail(EmailContent content, CancellationToken cancellationToken = default);
}