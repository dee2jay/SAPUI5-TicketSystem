using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Application.Services.MailService;

namespace TicketManagementSystem.Application.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailContent email, CancellationToken cancellationToken = default);
    }
}
