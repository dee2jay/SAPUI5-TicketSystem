using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketSystem.Domain.Models
{
    public class TicketAttachment
    {
        [Key]
        public Guid Id { get; set; }
        public int TicketId { get; set; }        
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public byte[] Data { get; set; } = [];
        public DateTime UploadedAt { get; set; }
    }
}
