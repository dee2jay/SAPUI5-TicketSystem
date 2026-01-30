using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;

namespace TicketManagementSystem.Domain.Models;

public class RefreshToken
{
    [Key]
    public Guid Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = default!;
    public string TokenHash { get; set; } = default!;
    public string DeviceId { get; set; } = default!;
    public Instant ExpiresAt { get; set; }
    public Instant? RevokedAt { get; set; }
    public bool IsValid =>
        RevokedAt == null && ExpiresAt > SystemClock.Instance.GetCurrentInstant();
}