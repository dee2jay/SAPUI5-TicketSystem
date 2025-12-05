using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;

namespace TicketManagementSystem.Domain.Models
{
    public class UserChangeLog
    {
        public string Title { get; set; } = string.Empty;
        public string Property { get; set; } = string.Empty;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public Instant? ChangedAt { get; set; }
        public string ChangedBy { get; set; } = string.Empty;
    }
}
