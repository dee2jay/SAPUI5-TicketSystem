using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Dtos;

public class TicketAttachmentDto
{
    public string FileName { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public Instant UploadAt { get; set; }
    public int TicketId { get; set; }
    public int UserId { get; set; }
}