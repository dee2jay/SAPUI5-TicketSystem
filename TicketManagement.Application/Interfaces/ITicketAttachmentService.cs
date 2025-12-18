using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Application.Interfaces
{
    public interface ITicketAttachmentService
    {
        public Task<TicketAttachment?> AddAttachment(int ticketId, IFormFile file, User user, CancellationToken ct);
        public Task<TicketAttachment> GetAttachmentById(string attachmentId, CancellationToken ct);
    }
}
