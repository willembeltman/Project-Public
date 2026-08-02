using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MediaScanner.Shared;
using MediaScanner.Shared.Models;
using MediaScanner.Shared.Enums;

namespace MediaScanner.Client;

public class TcpMediaServer
{
    private readonly int _port;
    private readonly MediaEngine _engine;
    private readonly ILogger _logger;
    private readonly string _clientId;
    private TcpListener? _listener;
    private readonly ConcurrentDictionary<Guid, NetworkStream> _controlConnections = new();
    private CancellationTokenSource? _cts;

    public TcpMediaServer(int port, string clientId, MediaEngine engine, ILogger logger)
    {
        _port = port;
        _clientId = clientId;
        _engine = engine;
        _logger = logger;

        _engine.OnFileUpdated += BroadcastFileUpdate;
    }

    public void Start()
    {
        _cts = new CancellationTokenSource();
        _listener = new TcpListener(IPAddress.Any, _port);
        _listener.Start();
        _logger.LogInformation("TCP Media Server listening on port {port}...", _port);

        _ = AcceptConnectionsAsync(_cts.Token);
    }

    public void Stop()
    {
        _cts?.Cancel();
        _listener?.Stop();
        
        foreach (var (id, stream) in _controlConnections)
        {
            try { stream.Dispose(); } catch { }
        }
        _controlConnections.Clear();
    }

    private async Task AcceptConnectionsAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                var tcpClient = await _listener!.AcceptTcpClientAsync(token);
                _ = HandleConnectionAsync(tcpClient, token);
            }
            catch (Exception ex) when (ex is not OperationCanceledException && ex is not ObjectDisposedException)
            {
                _logger.LogError(ex, "Error accepting TCP client.");
            }
        }
    }

    private async Task HandleConnectionAsync(TcpClient tcpClient, CancellationToken token)
    {
        using (tcpClient)
        {
            var stream = tcpClient.GetStream();
            try
            {
                // Read connection type (read up to a newline character or first few bytes)
                var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
                var headerLine = await reader.ReadLineAsync(token);
                if (string.IsNullOrEmpty(headerLine)) return;

                if (headerLine.Equals("CONTROL", StringComparison.OrdinalIgnoreCase))
                {
                    await HandleControlConnectionAsync(stream, token);
                }
                else if (headerLine.StartsWith("FILE:", StringComparison.OrdinalIgnoreCase))
                {
                    var requestPath = headerLine["FILE:".Length..].Trim();
                    await HandleFileConnectionAsync(stream, requestPath, token);
                }
                else
                {
                    _logger.LogWarning("Unknown connection header: {header}", headerLine);
                }
            }
            catch (Exception ex) when (ex is not OperationCanceledException && ex is not IOException)
            {
                _logger.LogError(ex, "Error handling client connection.");
            }
        }
    }

    private async Task HandleControlConnectionAsync(NetworkStream stream, CancellationToken token)
    {
        var connectionId = Guid.NewGuid();
        _controlConnections[connectionId] = stream;
        _logger.LogInformation("API Control connection established. Id: {id}", connectionId);

        try
        {
            // Send Handshake response
            var clientInfo = new ClientInfo
            {
                ClientId = _clientId,
                MachineName = Environment.MachineName
            };
            var handshakeMsg = new ControlMessage
            {
                Type = MessageType.HandshakeResponse,
                Payload = JsonSerializer.Serialize(clientInfo)
            };
            await NetworkHelper.SendJsonAsync(stream, handshakeMsg, token);

            // Send initial index sync
            var indexFiles = _engine.GetIndex();
            var syncMsg = new ControlMessage
            {
                Type = MessageType.IndexSync,
                Payload = JsonSerializer.Serialize(indexFiles)
            };
            await NetworkHelper.SendJsonAsync(stream, syncMsg, token);

            // Keep connection open and read heartbeats or wait for disconnection
            byte[] buffer = new byte[1024];
            while (!token.IsCancellationRequested)
            {
                // We just read to detect disconnection
                int read = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                if (read == 0)
                {
                    break;
                }
            }
        }
        finally
        {
            _controlConnections.TryRemove(connectionId, out _);
            _logger.LogInformation("API Control connection closed. Id: {id}", connectionId);
        }
    }

    private async Task HandleFileConnectionAsync(NetworkStream stream, string requestPath, CancellationToken token)
    {
        _logger.LogInformation("File stream requested for path: {path}", requestPath);

        var physicalPath = _engine.ResolvePhysicalPath(requestPath);
        if (physicalPath == null)
        {
            _logger.LogWarning("Requested file not found or path traversal blocked: {path}", requestPath);
            stream.WriteByte(0); // 0 = Failure
            await stream.FlushAsync(token);
            return;
        }

        try
        {
            using var fileStream = new FileStream(physicalPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
            var fileLength = fileStream.Length;

            stream.WriteByte(1); // 1 = Success
            byte[] lenBytes = BitConverter.GetBytes(fileLength);
            await stream.WriteAsync(lenBytes, 0, 8, token);

            await fileStream.CopyToAsync(stream, token);
            await stream.FlushAsync(token);
            _logger.LogInformation("Successfully streamed file: {path} ({size} bytes)", requestPath, fileLength);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error streaming file: {path}", requestPath);
            // If header was already sent, the stream is dirty, but we try to close gracefully.
        }
    }

    private void BroadcastFileUpdate(FileUpdateInfo updateInfo)
    {
        var msg = new ControlMessage
        {
            Type = MessageType.FileUpdate,
            Payload = JsonSerializer.Serialize(updateInfo)
        };

        foreach (var (id, stream) in _controlConnections)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    await NetworkHelper.SendJsonAsync(stream, msg);
                }
                catch
                {
                    // Failed connection will be handled/removed in its read loop
                }
            });
        }
    }
}
