using gAPI.Core.Server;
using gAPI.Core.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace UwvLlm.Core.Infrastructure.Data;

public class ApplicationDbContext : AuthenticationDbContext<User>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthUser).Assembly);
    }
}
