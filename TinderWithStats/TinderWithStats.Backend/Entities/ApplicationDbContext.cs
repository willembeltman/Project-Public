using gAPI.Core.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bsd.Infrastructure.Data.Entities;

public class ApplicationDbContext(DbContextOptions options)
    : AuthenticationDbContext<User>(options)
{
    public DbSet<Camera> Cameras { get; set; } = default!;
    public DbSet<CameraRecording> CameraRecordings { get; set; } = default!;
    public DbSet<CameraRecordingListener> CameraRecordingListeners { get; set; } = default!;
    public DbSet<Conversation> Conversations { get; set; } = default!;
    public DbSet<ConversationMessage> ConversationMessages { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CameraRecording>()
            .HasOne(cb => cb.Camera)
            .WithMany(cd => cd.CameraRecordings)
            .HasForeignKey(cb => cb.CameraId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CameraRecordingListener>()
            .HasOne(cb => cb.CameraRecording)
            .WithMany(cd => cd.CameraRecordingListeners)
            .HasForeignKey(cb => cb.CameraRecordingId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);


        //// Kijkt naar alle entiteiten en zet DateTime eigenschappen om naar UTC bij schrijven
        //foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        //{
        //    var properties = entityType.GetProperties()
        //        .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?));

        //    foreach (var property in properties)
        //    {
        //        property.SetValueConverter(new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTime, DateTime>(
        //            v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
        //            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)));
        //    }
        //}
    }
}
