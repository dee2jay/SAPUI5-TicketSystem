using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Interfaces;

public interface ITicketCommentService
{
    Task<TicketComment> AddCommentsByTicketId(int ticketId, TicketCommentDto dto, CancellationToken ct);
}