using gAPI.Core.Server;
using Microsoft.EntityFrameworkCore;

namespace UwvLlm.Core.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions options) : AuthenticationDbContext<User>(options)
{
    public virtual DbSet<MailMessage> MailMessages { get; set; }
    public virtual DbSet<UserNotification> UserNotifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // -------------------------
        // User
        // -------------------------
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasMany(u => u.Notifications)
                  .WithOne(n => n.User)
                  .HasForeignKey(n => n.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // -------------------------
        // UserNotification
        // -------------------------
        modelBuilder.Entity<UserNotification>(entity =>
        {
            entity.HasKey(n => n.Id);

            entity.Property(n => n.ExternalId)
                  .IsRequired();

            entity.Property(n => n.Title)
                  .IsRequired();

            entity.Property(n => n.Message)
                  .IsRequired();

            entity.Property(n => n.ExternalType)
                  .IsRequired();

            // ⚠️ string[] mapping (belangrijk punt)
            // Optie 1: JSON (aanbevolen)
            entity.Property(n => n.QuickOptions)
                  .HasConversion(
                      v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                      v => System.Text.Json.JsonSerializer.Deserialize<string[]>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? Array.Empty<string>()
                  );

            // Optioneel: kolomtype (bijv. nvarchar(max))
            entity.Property(n => n.QuickOptions)
                  .HasColumnType("nvarchar(max)");
        });

        // -------------------------
        // MailMessage
        // -------------------------
        modelBuilder.Entity<MailMessage>(entity =>
        {
            entity.HasKey(m => m.Id);

            entity.Property(m => m.Subject)
                  .IsRequired();

            entity.Property(m => m.Body)
                  .IsRequired();

            entity.Property(m => m.Date)
                  .IsRequired();

            // FromUser relatie (BELANGRIJK: dubbele relatie naar User)
            entity.HasOne(m => m.FromUser)
                  .WithMany()
                  .HasForeignKey(m => m.FromUserId)
                  .OnDelete(DeleteBehavior.Restrict); // voorkomt cascade loop

            // ToUser relatie
            entity.HasOne(m => m.ToUser)
                  .WithMany()
                  .HasForeignKey(m => m.ToUserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
