using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagementSystem.Domain.Models
{
    public class CategoryLocationUserMapping
    {
        [Key]
        public int Id { get; set; }
        public string? Category { get; set; }
        public string? Location  { get; set; }
        public string? MappedUser { get; set; }
    }
}
