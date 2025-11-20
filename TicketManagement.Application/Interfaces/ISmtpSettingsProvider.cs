using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Application.Services.MailService;

namespace TicketManagementSystem.Application.Interfaces;

public interface ISmtpSettingsProvider
{
    Task GenerateAndSendEmail(EmailContent content, CancellationToken cancellationToken = default);
}