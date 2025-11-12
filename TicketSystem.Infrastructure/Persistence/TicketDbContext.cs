using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Infrastructure.Persistence
{
    public class TicketDbContext : DbContext
    {
        public TicketDbContext(DbContextOptions<TicketDbContext> options) : base(options)
        {
        }
        public DbSet<TicketSystem.Domain.Models.TicketAttachment> TicketAttachments { get; set; }
        public DbSet<TicketSystem.Domain.Models.TicketComment> TicketComments { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>()
                .HasMany(t => t.Comments)
                .WithOne()
                .HasForeignKey(c => c.TicketId);
            
            modelBuilder.Entity<Ticket>()
                .HasMany(t => t.Attachments)
                .WithOne()
                .HasForeignKey(a => a.TicketId);
        }
    }
}
