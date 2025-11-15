using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Events;

public class UserCreatedEvent : IDomainEvent
{
    public int UserId { get; set; }
    public string Property { get; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    public DateTime OccurredOn { get; } = DateTime.Now;
    public string ChangedBy { get; set; } = string.Empty;
    
}