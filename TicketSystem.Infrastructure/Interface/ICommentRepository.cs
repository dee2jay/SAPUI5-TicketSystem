using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Infrastructure.Interface
{
    public interface ICommentRepository
    {
        public Task<bool> InsertComment(TicketComment comment);
    }
}
