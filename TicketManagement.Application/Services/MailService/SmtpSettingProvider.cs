using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Application.Interfaces;

namespace TicketManagementSystem.Application.Services.MailService
{
    public class SmtpSettingProvider : ISmtpSettingsProvider
    {
        private readonly SmtpSettingsOptions _options;

        private readonly IConfiguration _configuration;

        public SmtpSettingProvider(IConfiguration configuration, IOptions<SmtpSettingsOptions> options)
        {
            _configuration = configuration;
            _options = options.Value;
        }

        public async Task GenerateAndSendEmail(EmailContent content, CancellationToken cancellationToken = default)
        {
            using var message = new MailMessage
            {
                From = new MailAddress(_options.From),
                Subject = content.Subject,
                Body = content.Body,
                IsBodyHtml = content.IsHtml
            };

             content.To.ForEach(to =>
             {
                 message.To.Add(to);
             });

            using var client = new SmtpClient(_options.Host, _options.Port)
            {
                Credentials = new NetworkCredential(_options.Username, _options.Password),
                EnableSsl = _options.EnableSsl
            };

            cancellationToken.ThrowIfCancellationRequested();

            await client.SendMailAsync(message, cancellationToken);
        }
    }
}
