using System.Text.Json.Serialization;

namespace TicketManagementSystem.Domain.Models
{
    public class History
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }

        [JsonIgnore]
        public int TicketId { get; set; }

        [JsonIgnore]
        public Ticket Ticket { get; set; }
    }
}