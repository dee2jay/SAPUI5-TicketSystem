using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagementSystem.Application.Results;

public record RegisterUserResult(bool Success,
    int? UserId,
    string? ErrorCode,
    string? ErrorMessage)
{
    public static RegisterUserResult Ok(int userId)
        => new(true, userId, null, null);

    public static RegisterUserResult Fail(string code, string message)
        => new(false, null, code, message);
}