using System;

namespace ScrinkLargestVideos
{
    public class ApplicationDbContext : IDisposable
    {
        public DbSet<FileRapport> FileRapports { get; set; } = new DbSet<FileRapport>("FileRapports");
        public void Dispose()
        {
            FileRapports.Dispose();
        }
    }
}