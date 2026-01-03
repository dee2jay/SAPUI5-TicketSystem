using ErrorOr;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Infrastructure.Interface;

public interface ITicketRepository
{
    Task AddTicket(Ticket ticket);
    Task<ErrorOr<Ticket>> GetTicketById(int ticketId, CancellationToken ct);
    Task<ErrorOr<IEnumerable<Ticket>>> GetAllTickets(CancellationToken ct);
    Task UpdateTicket(Ticket ticket);
    Task<ErrorOr<IEnumerable<History>>> GetHistoryByTicketId(int ticketId, CancellationToken ct);
    Task<ErrorOr<IEnumerable<TicketAttachment>>> GetAttachmentsByTicketId(int ticketId, CancellationToken ct);
    Task<ErrorOr<IEnumerable<TicketComment>>> GetCommentsByTicketId(int ticketId, CancellationToken ct);
    Task RemoveAttachmentsByTicketId(int ticketId, string attachmentId, CancellationToken ct);
}