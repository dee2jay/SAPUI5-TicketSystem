using Microsoft.EntityFrameworkCore;
using NodaTime;
using TicketManagementSystem.Domain.Models;

namespace TicketManagementSystem.Infrastructure.Persistence;

public class TicketDbContext(DbContextOptions<TicketDbContext> options) : DbContext(options)
{
    public DbSet<TicketAttachment> TicketAttachments { get; set; }
    public DbSet<TicketComment> TicketComments { get; set; }
    public DbSet<History> TicketHistories { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryLocationUserMapping> CategoryLocationUserMappings { get; set; }

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
            .HasOne<User>()
            .WithMany(u => u.Attachments)
            .HasForeignKey(a => a.UserId);

        // -----------------------------
        // User - Comment (1 -> n)
        // -----------------------------
        modelBuilder.Entity<TicketComment>()
            .HasOne<User>()
            .WithMany(u => u.Comments)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.NoAction);
            

        // -----------------------------
        // Ticket - Attachment (1 -> n)
        // -----------------------------
        modelBuilder.Entity<TicketAttachment>()
            .HasOne<Ticket>() 
            .WithMany(u => u.Attachments) 
            .HasForeignKey(a => a.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        // -----------------------------
        // Ticket - Comment (1 -> n)
        // -----------------------------
        modelBuilder.Entity<TicketComment>()
            .HasOne<Ticket>()
            .WithMany(u => u.Comments) 
            .HasForeignKey(a => a.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        // -----------------------------
        // Ticket - History (1 -> n)
        // -----------------------------
        modelBuilder.Entity<History>()
            .HasOne<Ticket>()
            .WithMany(t => t.Histories)
            .HasForeignKey(h => h.TicketId)
            .OnDelete(DeleteBehavior.Cascade);
       
        // -----------------------------
        //              User
        // -----------------------------
        modelBuilder.Entity<User>();

        // -----------------------------
        //              Category
        // -----------------------------
        modelBuilder.Entity<Category>()
            .HasKey(c => c.Id);

        // -----------------------------
        //              Ticket
        // -----------------------------
        
        modelBuilder.Entity<Ticket>()
            .Property(t => t.CreatedAt)
            .HasConversion(
                v => v!.Value.ToDateTimeUtc(),
                v => Instant.FromDateTimeUtc(DateTime.SpecifyKind(v, DateTimeKind.Utc)));

        modelBuilder.Entity<Ticket>()
            .Property(t => t.UpdatedAt)
            .HasConversion(
                v => v!.Value.ToDateTimeUtc(),
                v => Instant.FromDateTimeUtc(DateTime.SpecifyKind(v, DateTimeKind.Utc)));

        modelBuilder.Entity<Ticket>()
            .Property(t => t.DueDate)
            .HasConversion(
                v => v!.Value.ToDateTimeUtc(),
                v => Instant.FromDateTimeUtc(DateTime.SpecifyKind(v, DateTimeKind.Utc)));

        // -----------------------------
        //              Attachments
        // -----------------------------
        modelBuilder.Entity<TicketAttachment>()
            .Property(t => t.UploadedAt)
            .HasConversion(
                v => v!.Value.ToDateTimeUtc(),
                v => Instant.FromDateTimeUtc(DateTime.SpecifyKind(v, DateTimeKind.Utc)));

        // -----------------------------
        //              Comment
        // -----------------------------
        modelBuilder.Entity<TicketComment>()
            .Property(t => t.CreatedAt)
            .HasConversion(
                v => v!.Value.ToDateTimeUtc(),
                v => Instant.FromDateTimeUtc(DateTime.SpecifyKind(v, DateTimeKind.Utc)));

        // -----------------------------
        //              History
        // -----------------------------
        modelBuilder.Entity<History>()
            .Property(t => t.Timestamp)
            .HasConversion(
                v => v!.Value.ToDateTimeUtc(),
                v => Instant.FromDateTimeUtc(DateTime.SpecifyKind(v, DateTimeKind.Utc)));

        // -----------------------------
        //              Category
        // -----------------------------
        modelBuilder.Entity<Category>();

        // -----------------------------
        //              Mappings 
        // -----------------------------
        modelBuilder.Entity<CategoryLocationUserMapping>();

    }
}