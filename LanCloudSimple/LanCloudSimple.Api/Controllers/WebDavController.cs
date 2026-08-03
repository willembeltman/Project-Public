using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using LanCloudSimple.Api.Models;
using LanCloudSimple.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;

namespace LanCloudSimple.Api.Controllers;

[ApiController]
[Route("dav")]
public class WebDavController : ControllerBase
{
    private readonly CloudService _mediaService;
    private readonly ILogger<WebDavController> _logger;

    public WebDavController(CloudService mediaService, ILogger<WebDavController> logger)
    {
        _mediaService = mediaService;
        _logger = logger;
    }

    [HttpOptions]
    [HttpOptions("{*path}")]
    public IActionResult Options()
    {
        Response.Headers.Append("DAV", "1, 2");
        Response.Headers.Append("Allow", "OPTIONS, GET, HEAD, PROPFIND, PUT, DELETE, MKCOL");
        Response.Headers.Append("MS-Author-Via", "DAV");
        return Ok();
    }

    [AcceptVerbs("PROPFIND")]
    [Route("{*path}")]
    public async Task<IActionResult> PropFind(string? path)
    {
        path = (path ?? "").Replace('\\', '/').Trim('/');
        var depthHeader = Request.Headers["Depth"].FirstOrDefault() ?? "1";

        _logger.LogInformation("WebDAV PROPFIND request for path: '{path}' (Depth: {depth})", path, depthHeader);

        // Get matching browse items
        // Wait, does the requested path exist in our index?
        // Let's check if the path itself is a directory, a file, or root.
        
        bool isRoot = string.IsNullOrEmpty(path);
        BrowseNode? targetFileNode = null;
        bool isDirectory = isRoot;

        if (!isRoot)
        {
            // Check if it exists as a file or folder
            // To check if it's a directory, we can browse its parent and check if this path is marked as IsDirectory
            var parentPath = Path.GetDirectoryName(path)?.Replace('\\', '/') ?? "";
            var parentItems = _mediaService.Browse(parentPath);
            var selfItem = parentItems.FirstOrDefault(x => x.Path.Equals(path, StringComparison.OrdinalIgnoreCase));

            if (selfItem != null)
            {
                isDirectory = selfItem.IsDirectory;
                if (!isDirectory)
                {
                    targetFileNode = selfItem;
                }
            }
            else
            {
                // Might be a directory containing other folders/files but not listed directly
                // E.g., we browse it directly. If we get children, then it is indeed a directory.
                var children = _mediaService.Browse(path);
                if (children.Any())
                {
                    isDirectory = true;
                }
                else
                {
                    return NotFound();
                }
            }
        }

        XNamespace d = "DAV:";
        var multistatus = new XElement(d + "multistatus");

        // Helper to add a response element
        void AddResponse(string hrefPath, string displayName, bool isDir, long size, DateTime? modTime)
        {
            // URL encode path segments
            var escapedHref = "/dav/" + string.Join("/", hrefPath.Split('/').Select(Uri.EscapeDataString));
            if (isDir && !escapedHref.EndsWith("/"))
            {
                escapedHref += "/";
            }

            var prop = new XElement(d + "prop");
            prop.Add(new XElement(d + "displayname", displayName));

            if (isDir)
            {
                prop.Add(new XElement(d + "resourcetype", new XElement(d + "collection")));
            }
            else
            {
                prop.Add(new XElement(d + "resourcetype"));
                prop.Add(new XElement(d + "getcontentlength", size));
                
                var contentTime = modTime ?? DateTime.UtcNow;
                prop.Add(new XElement(d + "getlastmodified", contentTime.ToString("R")));
                prop.Add(new XElement(d + "creationdate", contentTime.ToString("yyyy-MM-ddTHH:mm:ssZ")));
            }

            var response = new XElement(d + "response",
                new XElement(d + "href", escapedHref),
                new XElement(d + "propstat",
                    prop,
                    new XElement(d + "status", "HTTP/1.1 200 OK")
                )
            );

            multistatus.Add(response);
        }

        // 1. Add the target node itself
        if (isDirectory)
        {
            var dirName = isRoot ? "Root" : Path.GetFileName(path);
            AddResponse(path, dirName, true, 0, null);
        }
        else if (targetFileNode != null)
        {
            AddResponse(targetFileNode.Path, targetFileNode.Name, false, targetFileNode.Size, targetFileNode.MediaDate);
        }

        // 2. Add children if Depth is "1" and target is a directory
        if (isDirectory && depthHeader == "1")
        {
            var children = _mediaService.Browse(path);
            foreach (var child in children)
            {
                AddResponse(child.Path, child.Name, child.IsDirectory, child.Size, child.MediaDate);
            }
        }

        var xmlDoc = new XDocument(new XDeclaration("1.0", "utf-8", null), multistatus);
        var writer = new Utf8StringWriter();
        xmlDoc.Save(writer);

        return Content(writer.ToString(), "application/xml", Encoding.UTF8);
    }

    [HttpGet]
    [Route("{*path}")]
    public async Task<IActionResult> Get(string? path)
    {
        path = (path ?? "").Replace('\\', '/').Trim('/');
        if (string.IsNullOrEmpty(path))
        {
            // WebDAV client accessing root directory with GET. We can return empty or Ok.
            return Ok("WebDAV Root");
        }

        var resolved = _mediaService.ResolveMergedPath(path);
        if (resolved == null)
        {
            return NotFound();
        }

        var streamResult = await _mediaService.GetFileStreamAsync(resolved.Value.ClientId, resolved.Value.ClientPath);
        if (streamResult == null)
        {
            return NotFound();
        }

        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(path, out var contentType))
        {
            contentType = "application/octet-stream";
        }

        return File(streamResult.Value.Stream, contentType, Path.GetFileName(path), enableRangeProcessing: true);
    }

    [HttpPut("{*path}")]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> Put(string path)
    {
        path = path.Replace('\\', '/').Trim('/');
        _logger.LogInformation("WebDAV PUT request to upload file: {path}", path);

        try
        {
            await _mediaService.SaveFileAsync(path, Request.Body, CancellationToken.None);
            return Created($"/dav/{path}", null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "WebDAV PUT upload failed for {path}", path);
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{*path}")]
    public IActionResult Delete(string path)
    {
        path = path.Replace('\\', '/').Trim('/');
        _logger.LogInformation("WebDAV DELETE request for path: {path}", path);

        var resolved = _mediaService.ResolveMergedPath(path);
        if (resolved == null)
        {
            return NotFound();
        }

        if (!resolved.Value.ClientId.Equals("Local", StringComparison.OrdinalIgnoreCase))
        {
            // Clients are read-only in this system
            return Forbid("Cannot delete files hosted on remote clients.");
        }

        // Local storage file
        try
        {
            // Resolve actual path
            var localPath = Path.Combine(_mediaService.GetType().GetField("_localStoragePath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(_mediaService) as string ?? "", resolved.Value.ClientPath);
            var parentDir = Path.GetDirectoryName(localPath) ?? localPath;
            // Let's resolve the path robustly
            // Wait, we can get it from ResolveMergedPath. ClientPath starts with root name, so e.g. "LocalStorage/Uploaded/file.jpg"
            // Wait, parent of localStoragePath is the starting point. Let's do:
            var apiBase = AppContext.BaseDirectory; // We configured LocalStorage relative or absolute
            // Let's just find it via the resolved.Value.ClientPath
            // Actually, in MediaService, Local relative path is GetLocalRelativePath(fullPath), which starts with "LocalStorage/..."
            // So we can extract the path after "LocalStorage/":
            var relativePart = resolved.Value.ClientPath;
            if (relativePart.StartsWith("LocalStorage/", StringComparison.OrdinalIgnoreCase))
            {
                relativePart = relativePart["LocalStorage/".Length..];
            }
            
            // Re-resolve localStoragePath dynamically
            var storagePath = _mediaService.LocalStoragePath;
            if (string.IsNullOrEmpty(storagePath)) return BadRequest();

            var fullPath = Path.GetFullPath(Path.Combine(storagePath, relativePart));
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
                _logger.LogInformation("Successfully deleted local storage file: {path}", fullPath);
                return NoContent();
            }
            else if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, true);
                _logger.LogInformation("Successfully deleted local storage directory: {path}", fullPath);
                return NoContent();
            }
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "WebDAV DELETE failed for {path}", path);
            return StatusCode(500, ex.Message);
        }
    }

    [AcceptVerbs("MKCOL")]
    [Route("{*path}")]
    public IActionResult MakeCollection(string path)
    {
        path = path.Replace('\\', '/').Trim('/');
        _logger.LogInformation("WebDAV MKCOL request to create directory: {path}", path);

        try
        {
            var storagePath = _mediaService.LocalStoragePath;
            if (string.IsNullOrEmpty(storagePath)) return BadRequest();

            var fullPath = Path.GetFullPath(Path.Combine(storagePath, path));
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
                return Created($"/dav/{path}", null);
            }
            return Ok(); // Already exists
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "WebDAV MKCOL failed for {path}", path);
            return StatusCode(500, ex.Message);
        }
    }
}

public class Utf8StringWriter : StringWriter
{
    public override Encoding Encoding => Encoding.UTF8;
}
