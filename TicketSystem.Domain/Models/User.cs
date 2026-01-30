using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagementSystem.Domain.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public required string Email { get; set; }
    public required string Username { get; set; } = string.Empty;
    public required string Password { get; set; } = string.Empty;
    public List<Ticket> Tickets { get; set; } = [];
    public List<TicketAttachment> Attachments { get; set; } = [];
    public List<TicketComment> Comments { get; set; } = [];
}