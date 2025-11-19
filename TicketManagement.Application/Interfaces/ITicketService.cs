using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Interfaces;

public interface ITicketService
{
    public Task<Ticket> CreateTicketAsync(TicketDto dto, CancellationToken ct);
    public Task UpdateTicketAsync(int ticketId, TicketUpdateDto dto, CancellationToken ct);
    public Task<IEnumerable<Ticket>> GetAllTicketsAsync(CancellationToken ct);
    public Task<Ticket?> GetTicketById(int ticketId, CancellationToken ct);

}