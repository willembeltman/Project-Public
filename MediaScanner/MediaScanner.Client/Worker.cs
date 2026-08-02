using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MediaScanner.Client;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;
    private MediaEngine? _engine;
    private TcpMediaServer? _server;

    public Worker(ILogger<Worker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var clientId = _configuration["ClientConfig:ClientId"] ?? "Client-" + Guid.NewGuid().ToString()[..8];
        var portStr = _configuration["ClientConfig:Port"] ?? "5001";
        if (!int.TryParse(portStr, out int port))
        {
            port = 5001;
        }

        var scanDirs = _configuration.GetSection("ClientConfig:ScanDirectories").Get<List<string>>() ?? new List<string>();

        _logger.LogInformation("Initializing Media Scanner Client ({clientId}) on port {port}...", clientId, port);

        _engine = new MediaEngine(scanDirs, _logger);
        _engine.Start();

        _server = new TcpMediaServer(port, clientId, _engine, _logger);
        _server.Start();

        // Wait until host is stopped
        try
        {
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Worker is stopping...");
        }
        finally
        {
            _server.Stop();
            _engine.Stop();
            _logger.LogInformation("Worker stopped.");
        }
    }
}
