using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Infrastructure.Interface
{
    public interface ITicketCommentRepository
    {
        public Task<bool> InsertComment(TicketComment comment, CancellationToken ct);
    }
}
