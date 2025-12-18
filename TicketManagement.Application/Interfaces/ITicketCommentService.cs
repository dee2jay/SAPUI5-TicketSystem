using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Application.Dtos;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Interfaces
{
    public interface ITicketCommentService
    {
        Task<TicketComment> AddCommentsByTicketId(int ticketId, TicketCommentDto dto, CancellationToken ct);
    }
}
