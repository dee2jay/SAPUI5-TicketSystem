using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Infrastructure.Persistence;

public class TicketDbContext : DbContext
{
    public TicketDbContext(DbContextOptions<TicketDbContext> options) : base(options)
    {
    }
    public DbSet<TicketSystem.Domain.Models.TicketAttachment> TicketAttachments { get; set; }
    public DbSet<TicketComment> TicketComments { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost\sqlexpress;Database=TicketSystemDB;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;");
        }
    }

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