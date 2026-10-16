using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Rekenkamer.Infrastructure.Data.Entities;
using Rekenkamer.Infrastructure.Import;
using SmartComponents.LocalEmbeddings;

namespace Rekenkamer.ConsoleApp.Services;

public class ReaderService(
    PdfProcessor pdfProcessor,
    IDbContextFactory<ApplicationDbContext> dbFactory)
    : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var path = "..\\..\\..\\..\\assessment-ai-engineer-2026\\data";

        using var db = dbFactory.CreateDbContext();
        using var embedder = new LocalEmbedder();
        var dirinfo = new DirectoryInfo(path);
        await ReadRecursive(db, embedder, dirinfo, dirinfo);
    }

    private async Task ReadRecursive(ApplicationDbContext db, LocalEmbedder embedder, DirectoryInfo dirinfo, DirectoryInfo orgDirinfo)
    {
        var relativePath = dirinfo.FullName.Substring(orgDirinfo.FullName.Length).Replace("\\", "/");
        Console.WriteLine($"Inlezen van directory: {relativePath}");

        var dataSetName = dirinfo.Name;
        var files = dirinfo.GetFiles("*.pdf");
        foreach (var file in files)
        {
            var bytes = File.ReadAllBytes(file.FullName);
            var uploadFile = new Shared.Dtos.UploadFile(file.Name, relativePath, bytes);
            await pdfProcessor.ReadFile(db, embedder, uploadFile);
        }

        foreach (var subDir in dirinfo.GetDirectories())
        {
            await ReadRecursive(db, embedder, subDir, orgDirinfo);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
    }
}
