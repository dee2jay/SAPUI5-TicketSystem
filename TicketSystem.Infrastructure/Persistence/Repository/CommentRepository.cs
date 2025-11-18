using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;

namespace TicketManagementSystem.Infrastructure.Persistence.Repository
{
    internal class CommentRepository : ICommentRepository
    {
        private readonly TicketDbContext _dbContext;
        private readonly SemaphoreSlim _semaphore = new(1,1);

        public CommentRepository(TicketDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> InsertComment(TicketComment comment)
        {
            _semaphore.Wait();
            try
            {
                _dbContext.TicketComments.Add(comment);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
