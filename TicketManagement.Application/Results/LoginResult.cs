using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;

namespace TicketManagementSystem.Application.Results
{
    public record LoginResult(bool Success, string Token, string RefreshToken, Instant? ExpiresAt, string? Error, string? ErrorMessage)
    {
        public static LoginResult Ok(string token, string refreshToken, Instant expiresAt) =>
            new (true, token, refreshToken, expiresAt, null, null);

        public static LoginResult Fail(string errorCode, string errorMessage) => new(false, null, null, null, errorCode
            , errorMessage);
    }
}
