using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Domain.Enums;

namespace TicketManagementSystem.Application.Dtos
{
    public class TicketEditDto
    {
        public TicketStatus Status { get; set; }
        public TicketPriority Priority { get; set; }
        public string? AssignedTo { get; set; }
        public byte[] RowVersion { get; set; } = null!;
    }
}
