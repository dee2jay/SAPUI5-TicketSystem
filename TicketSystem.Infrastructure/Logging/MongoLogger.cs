using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.DbModels;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Infrastructure.Logging
{
    public class MongoLogger : IAppLogger
    {
        private readonly IMongoCollection<LogEntry> _logs;

        public MongoLogger(string? connectionString, string? databaseName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _logs = database.GetCollection<LogEntry>("Logs");
        }

        public async Task InsertLogAsync(string message, string level = "Info")
        {
            var entry = new LogEntry { Message = message, Level = level };
            await _logs.InsertOneAsync(entry);
        }

        async Task IAppLogger.LogError(string message, Exception ex, string? source, string stackTrace)
        {
            var details = ex != null ? $"{ex.Message}\n{ex.StackTrace}" : null;
            await InsertLogAsync("Error", message, source, details);
        }

        async Task IAppLogger.LogInfo(string message, string? source)
        {
            await InsertLogAsync("Info", message, source);
        }

        async Task IAppLogger.LogWarning(string message, string? source)
        {
            await InsertLogAsync("Warning", message, source);
        }

        private async Task InsertLogAsync(string level, string message, string? source, string? details = null)
        {
            var entry = new LogEntry
            {
                Level = level,
                Message = message,
                Source = source,
                Details = details
            };

            await _logs.InsertOneAsync(entry);
        }

        public async Task LogChangeAsync(TicketChangeLog logEntry)
        {
            await InsertLogAsync("Change", $"Ticket {logEntry.TicketId} changed by {logEntry.ChangedBy} at {logEntry.ChangedAt}", "TicketChange");
        }
    }
}
