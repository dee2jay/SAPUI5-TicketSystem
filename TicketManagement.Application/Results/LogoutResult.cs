using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;

namespace TicketManagementSystem.Application.Results
{
    public record LogoutResult(bool Success, Instant? Timestamp)
    {
        public static LogoutResult Ok() => new(true, SystemClock.Instance.GetCurrentInstant());
        public static LogoutResult Fail() => new(false, null);
    }
}
