using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagementSystem.Application.Services.MailService
{
    public class SmtpSettingsOptions
    {
        public string? Host { get; set; }
        public int Port { get; set; }
        public string? From { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool EnableSsl { get; set; }
    }
}
