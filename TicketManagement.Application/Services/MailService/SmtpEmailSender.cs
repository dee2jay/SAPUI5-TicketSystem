using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Application.Interfaces;

namespace TicketManagementSystem.Application.Services.MailService
{
    public class SmtpEmailSender : IEmailSender
    {
        public Task SendEmailAsync(EmailContent email, CancellationToken cancellationToken = default)
        {
            var mail = new MailMessage
            {
                From = email.From!,
                Subject = email.Subject,
                Body = email.Body,
                IsBodyHtml = email.IsBodyHtml
            };
            foreach (var address in email.To)
            {
                mail.To.Add(address);
            }

            using (var smtp = new SmtpClient())
            {
                 smtp.Send(mail);
            }

            return Task.CompletedTask;
        }
    }
}
