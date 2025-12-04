using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagementSystem.Application.Dtos;

public class TicketCommentDto
{
    public string? Author { get; set; }
    public string? Text { get; set; }
    public DateTime CreatedAt { get; set; }
}