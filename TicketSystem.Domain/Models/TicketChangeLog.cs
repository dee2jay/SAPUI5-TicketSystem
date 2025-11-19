using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagementSystem.Domain.Models;

public class TicketChangeLog
{
    public string? Title { get; set; }
    public int TicketId{get; set;}
    public string Property { get; set; } = string.Empty;
    public string? Value { get; set; }
    public DateTime ChangedAt { get; set; }
    public string ChangedBy { get; set; } = string.Empty;
}