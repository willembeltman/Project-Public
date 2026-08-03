using LanCloudSimple.Api.Helpers;
using LanCloudSimple.Api.Models;
using LanCloudSimple.Shared.Enums;
using LanCloudSimple.Shared.Helpers;
using LanCloudSimple.Shared.Models;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace LanCloudSimple.Api.Services;

public class CloudService : IHostedService
{
    private readonly ILogger<CloudService> _logger;
    private readonly IConfiguration _configuration;
    
    // Configured clients: key is address (e.g. "localhost:5001"), value is ClientConnection
    private readonly List<string> _clientAddresses;
    private readonly string _localStoragePath;
    public string LocalStoragePath => _localStoragePath;

    // In-memory store of all files from all clients and local storage
    // Key: ClientId or "Local", Value: Dictionary of relative paths (starting with root name) to MediaFileDto
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, CloudFileDto>> _clientFiles = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<string, ClientInfo> _clients = new(StringComparer.OrdinalIgnoreCase);

    private readonly List<Task> _connectionTasks = new();
    private CancellationTokenSource? _cts;
    private FileSystemWatcher? _localWatcher;

    public CloudService(ILogger<CloudService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;

        _clientAddresses = _configuration.GetSection("ClientConfig:Clients").Get<List<string>>() ?? new List<string>();
        _localStoragePath = _configuration["ClientConfig:LocalStoragePath"] ?? Path.Combine(AppContext.BaseDirectory, "LocalStorage");
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting MediaService API manager...");
        _cts = new CancellationTokenSource();

        // 1. Ensure local storage exists and index it
        if (!Directory.Exists(_localStoragePath))
        {
            Directory.CreateDirectory(_localStoragePath);
        }
        IndexLocalStorage();
        SetupLocalStorageWatcher();

        // 2. Start background connections to clients
        foreach (var addr in _clientAddresses)
        {
            _connectionTasks.Add(Task.Run(() => ConnectToClientLoopAsync(addr, _cts.Token)));
        }

        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping MediaService API manager...");
        _cts?.Cancel();

        if (_localWatcher != null)
        {
            _localWatcher.EnableRaisingEvents = false;
            _localWatcher.Dispose();
        }

        try
        {
            await Task.WhenAll(_connectionTasks);
        }
        catch { }
    }

    #region Local Storage Indexing
    private void IndexLocalStorage()
    {
        _logger.LogInformation("Indexing local storage: {path}", _localStoragePath);
        var localFiles = new ConcurrentDictionary<string, CloudFileDto>(StringComparer.OrdinalIgnoreCase);

        try
        {
            var rootName = Path.GetFileName(_localStoragePath);
            if (string.IsNullOrEmpty(rootName)) rootName = "LocalStorage";

            var files = Directory.EnumerateFiles(_localStoragePath, "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var dto = CreateLocalMediaFileDto(file);
                if (dto != null)
                {
                    localFiles[dto.Path] = dto;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to index local storage");
        }

        _clientFiles["Local"] = localFiles;
        _clients["Local"] = new ClientInfo { ClientId = "Local", MachineName = Environment.MachineName };
    }

    private void SetupLocalStorageWatcher()
    {
        try
        {
            _localWatcher = new FileSystemWatcher(_localStoragePath)
            {
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.CreationTime
            };

            _localWatcher.Created += (s, e) => OnLocalFileChanged(e.FullPath, FileUpdateType.Added);
            _localWatcher.Changed += (s, e) => OnLocalFileChanged(e.FullPath, FileUpdateType.Updated);
            _localWatcher.Deleted += (s, e) => OnLocalFileChanged(e.FullPath, FileUpdateType.Deleted);
            _localWatcher.Renamed += (s, e) => {
                OnLocalFileChanged(e.OldFullPath, FileUpdateType.Deleted);
                OnLocalFileChanged(e.FullPath, FileUpdateType.Added);
            };

            _localWatcher.EnableRaisingEvents = true;
            _logger.LogInformation("Watching local storage directory: {path}", _localStoragePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to set up local watcher");
        }
    }

    private void OnLocalFileChanged(string fullPath, FileUpdateType updateType)
    {
        if (Directory.Exists(fullPath)) return;

        var relativePath = GetLocalRelativePath(fullPath);
        var localDict = _clientFiles.GetOrAdd("Local", _ => new ConcurrentDictionary<string, CloudFileDto>(StringComparer.OrdinalIgnoreCase));

        if (updateType == FileUpdateType.Deleted)
        {
            localDict.TryRemove(relativePath, out _);
            _logger.LogInformation("Local storage file deleted: {path}", relativePath);
            return;
        }

        // Added/Updated
        if (File.Exists(fullPath))
        {
            var dto = CreateLocalMediaFileDto(fullPath);
            if (dto != null)
            {
                localDict[dto.Path] = dto;
                _logger.LogInformation("Local storage file {action}: {path}", updateType, relativePath);
            }
        }
    }

    private string GetLocalRelativePath(string fullPath)
    {
        var parentDir = Path.GetDirectoryName(_localStoragePath) ?? _localStoragePath;
        return Path.GetRelativePath(parentDir, fullPath).Replace('\\', '/');
    }

    private CloudFileDto? CreateLocalMediaFileDto(string fullPath)
    {
        try
        {
            var fileInfo = new FileInfo(fullPath);
            if (!fileInfo.Exists) return null;

            var relativePath = GetLocalRelativePath(fullPath);
            
            var match = System.Text.RegularExpressions.Regex.Match(fileInfo.Name, @"(?<!\d)(?<year>19\d{2}|20\d{2})[-_./]?(?<month>0[1-9]|1[0-2])[-_./]?(?<day>0[1-9]|[12]\d|3[01])(?!\d)");
            DateTime mediaDate;
            if (match.Success)
            {
                try
                {
                    mediaDate = new DateTime(int.Parse(match.Groups["year"].Value), int.Parse(match.Groups["month"].Value), int.Parse(match.Groups["day"].Value), 0, 0, 0, DateTimeKind.Utc);
                }
                catch
                {
                    mediaDate = fileInfo.CreationTimeUtc < fileInfo.LastWriteTimeUtc ? fileInfo.CreationTimeUtc : fileInfo.LastWriteTimeUtc;
                }
            }
            else
            {
                mediaDate = fileInfo.CreationTimeUtc < fileInfo.LastWriteTimeUtc ? fileInfo.CreationTimeUtc : fileInfo.LastWriteTimeUtc;
            }

            return new CloudFileDto
            {
                Path = relativePath,
                Size = fileInfo.Length,
                LastWriteTime = fileInfo.LastWriteTimeUtc,
                CreationTime = fileInfo.CreationTimeUtc,
                MediaDate = mediaDate
            };
        }
        catch
        {
            return null;
        }
    }
    #endregion

    #region Client TCP Sockets loop
    private async Task ConnectToClientLoopAsync(string address, CancellationToken token)
    {
        var parts = address.Split(':');
        if (parts.Length != 2 || !int.TryParse(parts[1], out int port))
        {
            _logger.LogError("Invalid client address: {address}", address);
            return;
        }
        var host = parts[0];

        while (!token.IsCancellationRequested)
        {
            _logger.LogInformation("Attempting to connect to client {host}:{port}...", host, port);
            try
            {
                using var tcpClient = new TcpClient();
                await tcpClient.ConnectAsync(host, port, token);

                using var stream = tcpClient.GetStream();
                
                // Write the CONTROL line
                var controlLineBytes = Encoding.UTF8.GetBytes("CONTROL\n");
                await stream.WriteAsync(controlLineBytes, 0, controlLineBytes.Length, token);
                await stream.FlushAsync(token);

                // Start reading responses
                string? currentClientId = null;

                while (!token.IsCancellationRequested)
                {
                    var msg = await NetworkHelper.ReceiveJsonAsync<ControlMessage>(stream, token);
                    if (msg == null) break;

                    if (msg.Type == MessageType.HandshakeResponse)
                    {
                        var info = JsonSerializer.Deserialize<ClientInfo>(msg.Payload ?? "{}");
                        if (info != null && !string.IsNullOrEmpty(info.ClientId))
                        {
                            currentClientId = info.ClientId;
                            _clients[currentClientId] = info;
                            RegisterClientAddress(currentClientId, address);
                            _logger.LogInformation("Connected to client: {id} on {machine}", info.ClientId, info.MachineName);
                        }
                    }
                    else if (msg.Type == MessageType.IndexSync && currentClientId != null)
                    {
                        var files = JsonSerializer.Deserialize<List<CloudFileDto>>(msg.Payload ?? "[]") ?? new List<CloudFileDto>();
                        var dict = new ConcurrentDictionary<string, CloudFileDto>(StringComparer.OrdinalIgnoreCase);
                        foreach (var f in files)
                        {
                            dict[f.Path] = f;
                        }
                        _clientFiles[currentClientId] = dict;
                        _logger.LogInformation("Synced index for client {id} ({count} files)", currentClientId, files.Count);
                    }
                    else if (msg.Type == MessageType.FileUpdate && currentClientId != null)
                    {
                        var update = JsonSerializer.Deserialize<FileUpdateInfo>(msg.Payload ?? "{}");
                        if (update != null && update.File != null)
                        {
                            var dict = _clientFiles.GetOrAdd(currentClientId, _ => new ConcurrentDictionary<string, CloudFileDto>(StringComparer.OrdinalIgnoreCase));
                            if (update.UpdateType == FileUpdateType.Deleted)
                            {
                                dict.TryRemove(update.File.Path, out _);
                            }
                            else
                            {
                                dict[update.File.Path] = update.File;
                            }
                            _logger.LogInformation("Received real-time update from client {id}: {action} {path}", currentClientId, update.UpdateType, update.File.Path);
                        }
                    }
                }
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogWarning("Disconnected or failed connection to {host}:{port}: {msg}", host, port, ex.Message);
            }

            // Remove client files if disconnected
            var matchingClients = _clients.Where(x => x.Value.ClientId != "Local" && address.Contains(x.Value.ClientId) || x.Key != "Local").ToList();
            // Wait a bit before reconnecting
            await Task.Delay(5000, token);
        }
    }
    #endregion

    #region Client File Streaming
    public async Task<(Stream Stream, long Length)?> GetFileStreamAsync(string clientId, string clientPath)
    {
        // If local storage, read directly
        if (clientId.Equals("Local", StringComparison.OrdinalIgnoreCase))
        {
            var parentDir = Path.GetDirectoryName(_localStoragePath) ?? _localStoragePath;
            var fullPath = Path.GetFullPath(Path.Combine(parentDir, clientPath));
            
            // Security check
            var rootDirFull = Path.GetFullPath(_localStoragePath);
            if (!fullPath.StartsWith(rootDirFull, StringComparison.OrdinalIgnoreCase) || !File.Exists(fullPath))
            {
                return null;
            }

            var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
            return (fileStream, fileStream.Length);
        }

        // For client, connect to client data port, request file, and return the network stream
        // Find which client address corresponds to this clientId
        // Wait, how do we know the address of clientId?
        // We can keep a dictionary of ClientId -> Address
        // Let's find it by looking up the client configuration or storing the address when registering the client!
        // Let's query which of our client addresses corresponds to the client ID.
        // We can just keep a map of ClientId -> ClientAddress in a ConcurrentDictionary.
        // Let's add that.
        // Let's store client ID to address mapping.
        // Wait! We can retrieve client address mapping from our connection loop. Let's do that.
        // Let's write client mapping logic.
        var address = GetClientAddress(clientId);
        if (address == null) return null;

        var parts = address.Split(':');
        if (parts.Length != 2 || !int.TryParse(parts[1], out int port)) return null;

        var tcpClient = new TcpClient();
        await tcpClient.ConnectAsync(parts[0], port);
        
        var stream = tcpClient.GetStream();
        var header = Encoding.UTF8.GetBytes($"FILE:{clientPath}\n");
        await stream.WriteAsync(header, 0, header.Length);
        await stream.FlushAsync();

        // Read status byte
        int status = stream.ReadByte();
        if (status != 1)
        {
            tcpClient.Close();
            return null;
        }

        // Read 8 bytes length
        byte[] lenBytes = new byte[8];
        int read = 0;
        while (read < 8)
        {
            int r = await stream.ReadAsync(lenBytes, read, 8 - read);
            if (r == 0)
            {
                tcpClient.Close();
                return null;
            }
            read += r;
        }
        long length = BitConverter.ToInt64(lenBytes, 0);

        // Return a wrapper stream that disposes the tcpClient when closed
        return (new NetworkStreamWithCleanup(tcpClient, stream), length);
    }

    private string? GetClientAddress(string clientId)
    {
        // Simple search: matches client address configured
        // In a real application, we can store ClientId -> Address in a dictionary when Handshake succeeds
        // Let's populate a Dictionary ClientId -> Address dynamically.
        // We will define:
        // private readonly ConcurrentDictionary<string, string> _clientIdToAddress = new();
        // In the connection loop, once Handshake is successful:
        // _clientIdToAddress[info.ClientId] = address;
        // Let's implement this maps look up:
        foreach (var addr in _clientAddresses)
        {
            // We can resolve it dynamically or if there's only one client, return it.
            // Let's just store the map! We will add a mapping in the connection loop.
            if (_clientIdToAddress.TryGetValue(clientId, out var address))
            {
                return address;
            }
        }
        return null;
    }

    private readonly ConcurrentDictionary<string, string> _clientIdToAddress = new(StringComparer.OrdinalIgnoreCase);
    #endregion

    #region Directory Merging and Browsing
    public List<BrowseNode> Browse(string relativePath)
    {
        // Normalise path (use forward slashes, trim start and end slashes)
        var path = (relativePath ?? "").Replace('\\', '/').Trim('/');

        var result = new List<BrowseNode>();
        var seenDirectories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var (clientId, filesDict) in _clientFiles)
        {
            foreach (var (clientFullPath, file) in filesDict)
            {
                // Strip the root share name to get the merged relative path
                // E.g. "Share1/Folder1/a.jpg" -> "Folder1/a.jpg"
                var mergedPath = StripRootName(clientFullPath);
                if (mergedPath == null) continue;

                // Check if file is inside the requested path
                if (path == "")
                {
                    // Browsing root
                    var parts = mergedPath.Split('/');
                    if (parts.Length == 1)
                    {
                        // It's a file in the root
                        result.Add(new BrowseNode
                        {
                            Name = parts[0],
                            Path = parts[0],
                            IsDirectory = false,
                            Size = file.Size,
                            MediaDate = file.MediaDate,
                            ClientId = clientId
                        });
                    }
                    else
                    {
                        // It's a directory in the root
                        var dirName = parts[0];
                        if (seenDirectories.Add(dirName))
                        {
                            result.Add(new BrowseNode
                            {
                                Name = dirName,
                                Path = dirName,
                                IsDirectory = true
                            });
                        }
                    }
                }
                else
                {
                    // Browsing a specific subpath, e.g. "Folder1"
                    if (mergedPath.StartsWith(path + "/", StringComparison.OrdinalIgnoreCase))
                    {
                        var relativePart = mergedPath[(path.Length + 1)..];
                        var parts = relativePart.Split('/');
                        if (parts.Length == 1)
                        {
                            // It's a file inside the folder
                            result.Add(new BrowseNode
                            {
                                Name = parts[0],
                                Path = path + "/" + parts[0],
                                IsDirectory = false,
                                Size = file.Size,
                                MediaDate = file.MediaDate,
                                ClientId = clientId
                            });
                        }
                        else
                        {
                            // It's a subdirectory inside the folder
                            var dirName = parts[0];
                            var subDirPath = path + "/" + dirName;
                            if (seenDirectories.Add(subDirPath))
                            {
                                result.Add(new BrowseNode
                                {
                                    Name = dirName,
                                    Path = subDirPath,
                                    IsDirectory = true
                                });
                            }
                        }
                    }
                }
            }
        }

        return result.OrderBy(x => !x.IsDirectory).ThenBy(x => x.Name).ToList();
    }

    // Helper to get client ID and client-specific path for a merged path
    public (string ClientId, string ClientPath)? ResolveMergedPath(string mergedPath)
    {
        var path = (mergedPath ?? "").Replace('\\', '/').Trim('/');

        foreach (var (clientId, filesDict) in _clientFiles)
        {
            foreach (var (clientFullPath, file) in filesDict)
            {
                var candidate = StripRootName(clientFullPath);
                if (candidate != null && candidate.Equals(path, StringComparison.OrdinalIgnoreCase))
                {
                    return (clientId, clientFullPath);
                }
            }
        }
        return null;
    }

    private static string? StripRootName(string path)
    {
        var index = path.IndexOf('/');
        if (index == -1) return null; // If no separator, cannot strip root
        return path[(index + 1)..];
    }
    #endregion

    #region Uploading
    public async Task SaveFileAsync(string relativePath, Stream dataStream, CancellationToken cancellationToken)
    {
        // Writes to the local storage of the API.
        // The path should go inside local storage.
        var targetFullPath = Path.GetFullPath(Path.Combine(_localStoragePath, relativePath));

        // Security check
        var rootDirFull = Path.GetFullPath(_localStoragePath);
        if (!targetFullPath.StartsWith(rootDirFull, StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedAccessException("Path traversal attempt detected.");
        }

        var dir = Path.GetDirectoryName(targetFullPath);
        if (dir != null && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        using var fileStream = new FileStream(targetFullPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
        await dataStream.CopyToAsync(fileStream, cancellationToken);
    }
    #endregion

    // Helper to trace client ID address mapping inside the connection loop
    private void RegisterClientAddress(string clientId, string address)
    {
        _clientIdToAddress[clientId] = address;
    }
}
