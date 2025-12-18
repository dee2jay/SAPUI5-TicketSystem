using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Infrastructure.Persistence.Repository
{
    public class TicketCommentRepository(TicketDbContext dbContext, IAppLogger logger) : ITicketCommentRepository
    {
        public async Task<bool> InsertComment(TicketComment comment, CancellationToken ct)
        {
            try
            {
                dbContext.TicketComments.Add(comment);
                await dbContext.SaveChangesAsync(ct);
                await logger.LogInfo($"Comment {comment.Text} has been added to the Ticket with ID {comment.TicketId}.");
                return true;
            }
            catch (DbException e)
            {
                await logger.LogError(e.Message, e, e.Source, e.StackTrace!);
                return false;
            }
        }
    }
}
