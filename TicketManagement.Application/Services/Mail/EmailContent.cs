using System.Net.Mail;

namespace TicketManagementSystem.Application.Services.Mail;

public class EmailContent
{
    public List<string>? To { get; set; }

    public string? Subject { get; set; }
    public string? Body { get; set; }
    public MailAddress? From { get; set; }
    public bool IsBodyHtml { get; set; } = true;
    public bool IsHtml { get; set; }
}