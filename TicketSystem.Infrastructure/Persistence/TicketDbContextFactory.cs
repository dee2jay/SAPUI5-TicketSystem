using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TicketManagementSystem.Infrastructure.Persistence
{
    public class TicketDbContextFactory : IDesignTimeDbContextFactory<TicketDbContext>
    {
        public TicketDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TicketDbContext>();
            optionsBuilder.UseSqlServer(@"Server=localhost\sqlexpress;Database=TicketSystemDB;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;");
            return new TicketDbContext(optionsBuilder.Options);
        }
    }
}
