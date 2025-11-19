using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagementSystem.Infrastructure.Interface;

public interface IAppLogger : IAsyncDisposable
{
    Task LogInfo(string message, string? source = null);
    Task LogWarning(string message, string? source = null);
    Task LogError(string message, Exception? ex, string? source, string stackTrace);
}