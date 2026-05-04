using gAPI.Core.Server;
using gAPI.Core.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace UwvLlm.Core.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions options) : AuthenticationDbContext<User>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthUser).Assembly);
    }

    public virtual DbSet<MailMessage> MailMessages { get; set; }
    public virtual DbSet<MailMessageToUser> MailMessageToUsers { get; set; }
}
