using gAPI.Storage.Server.EntityFrameworkDisk;

namespace ScrinkLargestVideos;

public class ApplicationDbContext : DbContext
{
    public DbSet<FileRapport> FileRapports { get; set; } = default!;
}