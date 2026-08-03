using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using LanCloudSimple.Shared.Models;
using LanCloudSimple.Shared.Enums;

namespace LanCloudSimple.Client.Processes;

public class CloudEngine
{
    private readonly ILogger _logger;
    private readonly List<string> _scanDirectories;
    private readonly ConcurrentDictionary<string, CloudFileDto> _index = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<FileSystemWatcher> _watchers = new();

    public event Action<FileUpdateInfo>? OnFileUpdated;

    // Matches dates like 2026-08-02, 2026_08_02, 20260802, etc.
    private static readonly Regex DateRegex = new(
        @"(?<!\d)(?<year>19\d{2}|20\d{2})[-_./]?(?<month>0[1-9]|1[0-2])[-_./]?(?<day>0[1-9]|[12]\d|3[01])(?!\d)",
        RegexOptions.Compiled);

    public CloudEngine(List<string> scanDirectories, ILogger logger)
    {
        _scanDirectories = scanDirectories;
        _logger = logger;
    }

    public List<CloudFileDto> GetIndex() => _index.Values.ToList();

    public void Start()
    {
        _logger.LogInformation("Starting media indexing and file system watchers...");
        foreach (var dir in _scanDirectories)
        {
            if (!Directory.Exists(dir))
            {
                _logger.LogWarning("Directory does not exist: {dir}", dir);
                continue;
            }

            // Index existing files
            IndexDirectory(dir);

            // Set up file system watcher
            try
            {
                var watcher = new FileSystemWatcher(dir)
                {
                    IncludeSubdirectories = true,
                    NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.CreationTime
                };

                watcher.Created += (s, e) => OnFileChanged(dir, e.FullPath, FileUpdateType.Added);
                watcher.Changed += (s, e) => OnFileChanged(dir, e.FullPath, FileUpdateType.Updated);
                watcher.Deleted += (s, e) => OnFileChanged(dir, e.FullPath, FileUpdateType.Deleted);
                watcher.Renamed += (s, e) => {
                    OnFileChanged(dir, e.OldFullPath, FileUpdateType.Deleted);
                    OnFileChanged(dir, e.FullPath, FileUpdateType.Added);
                };

                watcher.EnableRaisingEvents = true;
                _watchers.Add(watcher);
                _logger.LogInformation("Watching directory: {dir}", dir);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to set up watcher for: {dir}", dir);
            }
        }
    }

    public void Stop()
    {
        foreach (var watcher in _watchers)
        {
            watcher.EnableRaisingEvents = false;
            watcher.Dispose();
        }
        _watchers.Clear();
    }

    private void IndexDirectory(string dir)
    {
        _logger.LogInformation("Indexing directory: {dir}", dir);
        try
        {
            var files = Directory.EnumerateFiles(dir, "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var dto = CreateMediaFileDto(dir, file);
                if (dto != null)
                {
                    _index[dto.Path] = dto;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error indexing directory: {dir}", dir);
        }
    }

    private void OnFileChanged(string rootDir, string fullPath, FileUpdateType updateType)
    {
        if (Directory.Exists(fullPath)) return; // Ignore directories

        var relativePath = GetRelativePath(rootDir, fullPath);

        if (updateType == FileUpdateType.Deleted)
        {
            if (_index.TryRemove(relativePath, out var removed))
            {
                _logger.LogInformation("File deleted: {path}", relativePath);
                OnFileUpdated?.Invoke(new FileUpdateInfo { UpdateType = FileUpdateType.Deleted, File = removed });
            }
            return;
        }

        // Added or Updated
        // Give a tiny delay or retry mechanism if file is still being written to by another process
        CloudFileDto? dto = null;
        for (int i = 0; i < 3; i++)
        {
            try
            {
                if (!File.Exists(fullPath)) return;
                dto = CreateMediaFileDto(rootDir, fullPath);
                break;
            }
            catch (IOException)
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        if (dto != null)
        {
            _index[dto.Path] = dto;
            _logger.LogInformation("File {action}: {path} (Date: {date})", updateType, dto.Path, dto.MediaDate);
            OnFileUpdated?.Invoke(new FileUpdateInfo { UpdateType = updateType, File = dto });
        }
    }

    public string? ResolvePhysicalPath(string requestPath)
    {
        // Request path starts with "ShareName/..."
        // We find which scan directory matches this share name
        foreach (var dir in _scanDirectories)
        {
            var rootName = Path.GetFileName(dir);
            if (string.IsNullOrEmpty(rootName)) continue;

            if (requestPath.StartsWith(rootName + "/", StringComparison.OrdinalIgnoreCase) ||
                requestPath.StartsWith(rootName + "\\", StringComparison.OrdinalIgnoreCase))
            {
                var relativePart = requestPath[rootName.Length..].TrimStart('/', '\\');
                var physicalPath = Path.GetFullPath(Path.Combine(dir, relativePart));
                
                // Security check to avoid path traversal
                var dirFullPath = Path.GetFullPath(dir);
                if (physicalPath.StartsWith(dirFullPath, StringComparison.OrdinalIgnoreCase) && File.Exists(physicalPath))
                {
                    return physicalPath;
                }
            }
        }
        return null;
    }

    private string GetRelativePath(string rootDir, string fullPath)
    {
        var parentDir = Path.GetDirectoryName(rootDir) ?? rootDir;
        return Path.GetRelativePath(parentDir, fullPath).Replace('\\', '/');
    }

    private CloudFileDto? CreateMediaFileDto(string rootDir, string fullPath)
    {
        try
        {
            var fileInfo = new FileInfo(fullPath);
            if (!fileInfo.Exists) return null;

            var relativePath = GetRelativePath(rootDir, fullPath);
            var mediaDate = DetermineMediaDate(fileInfo);

            return new CloudFileDto
            {
                Path = relativePath,
                Size = fileInfo.Length,
                LastWriteTime = fileInfo.LastWriteTimeUtc,
                CreationTime = fileInfo.CreationTimeUtc,
                MediaDate = mediaDate
            };
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Failed to read file info for {path}: {msg}", fullPath, ex.Message);
            return null;
        }
    }

    public static DateTime DetermineMediaDate(FileInfo fileInfo)
    {
        var name = fileInfo.Name;
        var match = DateRegex.Match(name);
        if (match.Success)
        {
            var year = int.Parse(match.Groups["year"].Value);
            var month = int.Parse(match.Groups["month"].Value);
            var day = int.Parse(match.Groups["day"].Value);

            try
            {
                return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
            }
            catch (ArgumentOutOfRangeException)
            {
                // Invalid date numbers (e.g. 2026-02-31)
            }
        }

        // Fallback: earliest of creation time and last write time
        var cTime = fileInfo.CreationTimeUtc;
        var wTime = fileInfo.LastWriteTimeUtc;
        return cTime < wTime ? cTime : wTime;
    }
}
