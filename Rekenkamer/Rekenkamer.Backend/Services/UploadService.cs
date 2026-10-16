using gAPI.Generated;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Rekenkamer.Infrastructure.Data.Entities;
using Rekenkamer.Infrastructure.Import;
using Rekenkamer.Shared.Dtos;
using SmartComponents.LocalEmbeddings;
using System.Threading.Channels;

namespace Rekenkamer.Backend.Services;

/// <summary>
/// Singleton
/// </summary>
public class UploadService(
    PdfProcessor pdfProcessor,
    IServiceScopeFactory scopeFactory)
{
    private long CurrentIndex;
    private Channel<UploadFile> FileChannel = Channel.CreateUnbounded<UploadFile>();

    public string? Status { get; set; }

    void SendAndSetStatus(Shared.Interfaces.IUploadHub toAll, string value)
    {
        Status = value;
        toAll.UpdateStatus(++CurrentIndex, $"{DateTime.Now} {value}");
    }

    public async Task Start(CancellationToken ct)
    {
        Status = "Upload service started";
        using var embedder = new LocalEmbedder();
        await foreach (var file in FileChannel.Reader.ReadAllAsync(ct))
        {
            using var scope = scopeFactory.CreateScope();
            var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
            using var db = dbContextFactory.CreateDbContext();
            var clientContext = scope.ServiceProvider.GetRequiredService<IClientContext>();

            SendAndSetStatus(clientContext.UploadHub.ToAll, $"Start reading {file.RelativePath}/{file.Name}");
            try
            {
                CurrentIndex = await pdfProcessor.ReadFile(db, embedder, file, CurrentIndex, clientContext.UploadHub.ToAll);
            }
            catch (Exception ex)
            {
                SendAndSetStatus(clientContext.UploadHub.ToAll, $"Error reading {file.RelativePath}/{file.Name}: " + ex.Message);
            }

            if (FileChannel.Reader.CanCount && FileChannel.Reader.Count > 0)
                continue;

            SendAndSetStatus(clientContext.UploadHub.ToAll, $"Idle");
        }
    }

    public ValueTask AddFile(UploadFile uploadFile, CancellationToken ct)
    {
        return FileChannel.Writer.WriteAsync(uploadFile, ct);
    }
}
