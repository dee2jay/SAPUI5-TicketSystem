using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagementSystem.Application.Services.MailService
{
    public class EmailContent
    {
        public List<string> To { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }
        public MailAddress? From { get; set; }
        public bool IsBodyHtml { get; set; } = true;
        public bool IsHtml { get; set; }
    }
}
