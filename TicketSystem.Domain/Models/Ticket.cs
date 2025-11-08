using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketSystem.Domain.Enums;

namespace TicketSystem.Domain.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }       
        public TicketStatus Status { get; set; }
        public TicketPriority Priority { get; set; }
        
        [Required]
        public string Category { get; set; } = string.Empty;
        public string? Location { get; set; }
        public string? CostCenter { get; set; }

        public string? AssignedTo { get; set; }

        public ICollection<TicketAttachment> Attachments { get; set; } = [];

        public ICollection<TicketComment> Comments { get; set; } = [];
    }
}
