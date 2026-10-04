using gAPI.Core.Attributes;
using gAPI.Core.Server.Entities;
using gAPI.Core.Server.Storage;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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
        // CRUCIAAL: Zorg ervoor dat de Identity/Authentication tabellen correct worden opgebouwd
        base.OnModelCreating(modelBuilder);

        // 1. Relatie: User <-> Profile (Een User kan meerdere Profiles hebben)
        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasOne(p => p.User)
                  .WithMany(u => u.Profiles)
                  .HasForeignKey(p => p.UserId)
                  .OnDelete(DeleteBehavior.Cascade); // Als een User wordt verwijderd, verdwijnen ook de profielen
        });

        // 2. Relatie: Profile <-> ProfilePicture (Een Profile heeft meerdere ProfilePictures)
        modelBuilder.Entity<ProfilePicture>(entity =>
        {
            entity.HasOne(p => p.Profile)
                  .WithMany(p => p.ProfilePictures)
                  .HasForeignKey(p => p.ProfileId)
                  .OnDelete(DeleteBehavior.Cascade); // Als een Profile wordt verwijderd, verdwijnen de foto's
        });

        // 3. Relatie: Location <-> Profile (Een Location heeft meerdere Profiles)
        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasOne(p => p.Location)
                  .WithMany(l => l.Profiles)
                  .HasForeignKey(p => p.LocationId)
                  .OnDelete(DeleteBehavior.Restrict); // Voorkomt dat een locatie per ongeluk wordt verwijderd als er nog profielen aan gekoppeld zijn
        });
    }
}


