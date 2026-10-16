using gAPI.Core.Server.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Rekenkamer.Infrastructure.Data.Extensions;

namespace Rekenkamer.Infrastructure.Data.Entities;

public class ApplicationDbContext(
    DbContextOptions options,
    IConfiguration configuration) 
    : AuthenticationDbContext<User>(options)
{
    public DbSet<Document> Documents { get; set; } = default!;
    public DbSet<DocumentChunk> DocumentChunks { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DocumentChunk>()
            .HasOne(cb => cb.Document)
            .WithMany(cd => cd.Chunks)
            .HasForeignKey(cb => cb.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Zorg dat de pgvector extensie wordt aangemaakt in Postgres
        modelBuilder.HasPostgresExtension("vector");

        //modelBuilder.Entity<DocumentChunk>()
        //    .Property(c => c.Vector)
        //    .HasColumnType("vector(768)");

        // Haal hier dynamisch de waarde op uit je appsettings via je bestaande extensie-methode
        var llmConfig = CreateLlmConfigExtension.CreateLlmConfig(configuration);
        var vectorDimensions = llmConfig.VectorDimensions; // Zorg dat deze property bestaat (bijv. 768)

        modelBuilder.Entity<DocumentChunk>()
            .Property(c => c.Vector)
            .HasColumnType($"vector({vectorDimensions})");
    }
}
