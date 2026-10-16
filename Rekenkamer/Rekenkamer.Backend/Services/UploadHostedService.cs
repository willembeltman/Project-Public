namespace Rekenkamer.Backend.Services;

public class UploadHostedService(
    UploadService uploadService,
    ILoggerFactory loggerFactory)
    : IHostedService
{
    private readonly ILogger<UploadHostedService> Logger = loggerFactory.CreateLogger<UploadHostedService>();

    async Task IHostedService.StartAsync(CancellationToken ct)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                await uploadService.Start(ct);
            }
            catch(Exception ex) 
            {
                Logger.LogError(ex, "Error while starting upload service");
            }
        });
    }

    async Task IHostedService.StopAsync(CancellationToken ct)
    {
    }
}
