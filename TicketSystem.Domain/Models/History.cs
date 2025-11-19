namespace TicketManagementSystem.Domain.Models
{
    public class History
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }

        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
    }
}