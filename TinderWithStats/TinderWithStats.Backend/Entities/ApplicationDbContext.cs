using gAPI.Core.Server.Entities;

namespace TinderWithStats.Backend.Entities;

using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : AuthenticationDbContext<User>(options)
{
    // DbSets voor jouw entiteiten (User hoeft hier vaak niet bij omdat deze al in de base context zit, maar mag wel)
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<ProfilePicture> ProfilePictures { get; set; }
    public DbSet<Location> Locations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasOne(p => p.User)
                  .WithMany(u => u.Profiles)
                  .HasForeignKey(p => p.UserId)
                  .OnDelete(DeleteBehavior.Cascade); // Als een User wordt verwijderd, verdwijnen ook de profielen
        });

        modelBuilder.Entity<ProfilePicture>(entity =>
        {
            entity.HasOne(p => p.Profile)
                  .WithMany(p => p.ProfilePictures)
                  .HasForeignKey(p => p.ProfileId)
                  .OnDelete(DeleteBehavior.Cascade); // Als een Profile wordt verwijderd, verdwijnen de foto's
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasOne(p => p.Location)
                  .WithMany(l => l.Profiles)
                  .HasForeignKey(p => p.LocationId)
                  .OnDelete(DeleteBehavior.NoAction); // Voorkomt dat een locatie per ongeluk wordt verwijderd als er nog profielen aan gekoppeld zijn
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasOne(p => p.ProfileSender)
                  .WithMany(l => l.MatchesSend)
                  .HasForeignKey(p => p.ProfileSenderId)
                  .OnDelete(DeleteBehavior.NoAction); // Voorkomt dat een locatie per ongeluk wordt verwijderd als er nog profielen aan gekoppeld zijn
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasOne(p => p.ProfileReceiver)
                  .WithMany(l => l.MatchesReceived)
                  .HasForeignKey(p => p.ProfileReceiverId)
                  .OnDelete(DeleteBehavior.NoAction); // Voorkomt dat een locatie per ongeluk wordt verwijderd als er nog profielen aan gekoppeld zijn
        });

        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasOne(p => p.Match)
                  .WithMany(l => l.ChatMessages)
                  .HasForeignKey(p => p.MatchId)
                  .OnDelete(DeleteBehavior.NoAction); // Voorkomt dat een locatie per ongeluk wordt verwijderd als er nog profielen aan gekoppeld zijn
        });

        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasOne(p => p.ProfileReceiver)
                  .WithMany(l => l.MessagesReceived)
                  .HasForeignKey(p => p.ProfileReceiverId)
                  .OnDelete(DeleteBehavior.NoAction); // Voorkomt dat een locatie per ongeluk wordt verwijderd als er nog profielen aan gekoppeld zijn
        });
        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasOne(p => p.ProfileSender)
                  .WithMany(l => l.MessagesSend)
                  .HasForeignKey(p => p.ProfileSenderId)
                  .OnDelete(DeleteBehavior.NoAction); // Voorkomt dat een locatie per ongeluk wordt verwijderd als er nog profielen aan gekoppeld zijn
        });
    }
}


