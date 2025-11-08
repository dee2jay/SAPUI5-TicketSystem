using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketSystem.Domain.Models
{
    public class TicketComment
    {
        [Key]
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string? Author { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
