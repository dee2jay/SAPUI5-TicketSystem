using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TicketManagementSystem.Infrastructure.DbModels;

public class LogEntry
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        
    [BsonElement("level")]
    public string Level { get; set; } = "Info";
        
    [BsonElement("message")]
    public string Message { get; set; } = string.Empty;
        
    [BsonElement("source")]
    public string? Source { get; set; }
        
    [BsonElement("stackTrace")]
    public string StackTrace { get; set; } = string.Empty;

    [BsonElement("details")]
    public string? Details { get; set; }

    [BsonElement("userId")]
    public string? UserId { get; set; }
}