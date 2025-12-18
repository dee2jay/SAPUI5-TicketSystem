using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Infrastructure.Interface
{
    public interface ITicketAttachmentRepository
    {
        Task<ErrorOr<TicketAttachment>> GetTicketAttachmentById(string id, CancellationToken ct);
        Task AddAttachmentAsync(TicketAttachment  attachment, CancellationToken ct);
    }
}
