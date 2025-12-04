using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagementSystem.Application.Dtos;

public class TicketAttachmentDto
{
    public string FileName { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public byte[] Data { get; set; } = [];
    public DateTime UploadAt { get; set; }
}