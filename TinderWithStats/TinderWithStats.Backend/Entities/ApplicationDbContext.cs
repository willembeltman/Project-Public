using gAPI.Core.Server.Entities;

namespace TinderWithStats.Backend.Entities;

using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : AuthenticationDbContext<User>(options)
{
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<ProfilePicture> ProfilePictures { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasOne(p => p.User)
                  .WithMany(l => l.UserRoles)
                  .HasForeignKey(p => p.UserId)
                  .OnDelete(DeleteBehavior.NoAction);
        });
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasOne(p => p.Role)
                  .WithMany(l => l.UserRoles)
                  .HasForeignKey(p => p.RoleId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasOne(p => p.User)
                  .WithMany(u => u.Profiles)
                  .HasForeignKey(p => p.UserId)
                  .OnDelete(DeleteBehavior.Cascade); 
        });

        modelBuilder.Entity<ProfilePicture>(entity =>
        {
            entity.HasOne(p => p.Profile)
                  .WithMany(p => p.ProfilePictures)
                  .HasForeignKey(p => p.ProfileId)
                  .OnDelete(DeleteBehavior.Cascade); 
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasOne(p => p.Location)
                  .WithMany(l => l.Profiles)
                  .HasForeignKey(p => p.LocationId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasOne(p => p.ProfileSender)
                  .WithMany(l => l.MatchesSend)
                  .HasForeignKey(p => p.ProfileSenderId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasOne(p => p.ProfileReceiver)
                  .WithMany(l => l.MatchesReceived)
                  .HasForeignKey(p => p.ProfileReceiverId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasOne(p => p.Match)
                  .WithMany(l => l.ChatMessages)
                  .HasForeignKey(p => p.MatchId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasOne(p => p.ProfileReceiver)
                  .WithMany(l => l.MessagesReceived)
                  .HasForeignKey(p => p.ProfileReceiverId)
                  .OnDelete(DeleteBehavior.NoAction);
        });
        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasOne(p => p.ProfileSender)
                  .WithMany(l => l.MessagesSend)
                  .HasForeignKey(p => p.ProfileSenderId)
                  .OnDelete(DeleteBehavior.NoAction);
        });
    }
}


