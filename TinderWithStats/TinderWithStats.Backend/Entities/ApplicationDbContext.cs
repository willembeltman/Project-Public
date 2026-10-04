using gAPI.Core.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bsd.Infrastructure.Data.Entities;

public class ApplicationDbContext(DbContextOptions options)
    : AuthenticationDbContext<User>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}
