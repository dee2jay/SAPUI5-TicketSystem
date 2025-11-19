using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Infrastructure.Persistence;

public class TicketDbContext : DbContext
{
    public TicketDbContext(DbContextOptions<TicketDbContext> options) : base(options)
    {
    }
    public DbSet<TicketAttachment> TicketAttachments { get; set; }
    public DbSet<TicketComment> TicketComments { get; set; }
    public DbSet<History> TicketHistories { get; set; }
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
        // -----------------------------
        // User - Ticket (1 -> n)
        // -----------------------------
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.User)
            .WithMany(u => u.Tickets)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        // -----------------------------
        // User - Attachment (1 -> n)
        // -----------------------------
        modelBuilder.Entity<TicketAttachment>()
            .HasOne(a => a.User)
            .WithMany(u => u.Attachments)
            .HasForeignKey(a => a.UserId);

        // -----------------------------
        // User - Comment (1 -> n)
        // -----------------------------
        modelBuilder.Entity<TicketComment>()
            .HasOne(a => a.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.NoAction);
            

        // -----------------------------
        // Ticket - Attachment (1 -> n)
        // -----------------------------
        modelBuilder.Entity<TicketAttachment>()
            .HasOne(a => a.Ticket) 
            .WithMany(u => u.Attachments) 
            .HasForeignKey(a => a.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        // -----------------------------
        // Ticket - Comment (1 -> n)
        // -----------------------------
        modelBuilder.Entity<TicketComment>()
            .HasOne(a => a.Ticket)
            .WithMany(u => u.Comments) 
            .HasForeignKey(a => a.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        // -----------------------------
        // Ticket - History (1 -> n)
        // -----------------------------
        modelBuilder.Entity<History>()
            .HasOne(h => h.Ticket)
            .WithMany(t => t.Histories)
            .HasForeignKey(h => h.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        // -----------------------------
        //              User
        // -----------------------------
        modelBuilder.Entity<User>();

        // -----------------------------
        //              Ticket
        // -----------------------------
        modelBuilder.Entity<Ticket>();

        // -----------------------------
        //              Attachments
        // -----------------------------
        modelBuilder.Entity<TicketAttachment>();

        // -----------------------------
        //              Comment
        // -----------------------------
        modelBuilder.Entity<TicketComment>();

        // -----------------------------
        //              History
        // -----------------------------
        modelBuilder.Entity<History>();

    }
}