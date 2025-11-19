using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.DbModels;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Infrastructure.Logging;

public class MongoLogger : IAppLogger
{
    private readonly IMongoCollection<LogEntry> _logs;
        
    public MongoLogger(string? connectionString, string? databaseName)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException(nameof(connectionString));
        }

        var mongoUrl = MongoUrl.Create(connectionString);
        var client = new MongoClient(mongoUrl);

        // Resolve database name:
        string? dbName = databaseName;
        if (string.IsNullOrWhiteSpace(dbName))
        {
            dbName = mongoUrl.DatabaseName;
        }

        // If dbName still looks like a URI, parse it
        if (!string.IsNullOrWhiteSpace(dbName) &&
            (dbName.StartsWith("mongodb://", StringComparison.OrdinalIgnoreCase) ||
             dbName.StartsWith("mongodb+srv://", StringComparison.OrdinalIgnoreCase)))
        {
            dbName = MongoUrl.Create(dbName).DatabaseName;
        }

        if (string.IsNullOrWhiteSpace(dbName))
            throw new ArgumentException("Database name must be provided either in connectionString or databaseName.");

        var database = client.GetDatabase(dbName);
        _logs = database.GetCollection<LogEntry>("Logs");
    }

    async Task IAppLogger.LogError(string message, Exception? ex, string? source, string stackTrace)
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


    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }
}