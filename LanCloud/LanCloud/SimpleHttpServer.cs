using System.IO;
using System.Net;
using System.Text;
using System;

public class SimpleHttpServer
{
    private readonly HttpListener _listener;
    private readonly string _baseFolder;

    public SimpleHttpServer(string prefix, string baseFolder)
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add(prefix);
        _baseFolder = baseFolder;
    }

    public void Start()
    {
        _listener.Start();
        Console.WriteLine("Server gestart op: '" + string.Join("', '", _listener.Prefixes) + "'");
        while (true)
        {
            var context = _listener.GetContext();
            HandleRequest(context);
        }
    }

    private void HandleRequest(HttpListenerContext context)
    {
        if (context.Request.HttpMethod == "OPTIONS")
        {
            HandleOptions(context);
        }
        else if (context.Request.HttpMethod == "PROPFIND")
        {
            HandlePropFind(context);
        }
        else
        {
            var requestedUrl = context.Request.Url.AbsolutePath;
            var localPath = Path.Combine(_baseFolder, requestedUrl.TrimStart('/'));

            if (Directory.Exists(localPath))
            {
                HandleDirectoryListing(context, localPath);
            }
            else if (File.Exists(localPath))
            {
                HandleFileRequest(context, localPath);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                byte[] message = Encoding.UTF8.GetBytes("Bestand of map niet gevonden.");
                context.Response.OutputStream.Write(message, 0, message.Length);
            }
            context.Response.OutputStream.Close();
        }
    }
    private void HandleDirectoryListing(HttpListenerContext context, string directoryPath)
    {
        var files = Directory.GetFiles(directoryPath);
        var directories = Directory.GetDirectories(directoryPath);

        StringBuilder responseText = new StringBuilder();
        responseText.Append("<html><body><h2>Inhoud van map:</h2><ul>");

        // Toon directories
        foreach (var directory in directories)
        {
            var dirName = Path.GetFileName(directory);
            responseText.AppendFormat("<li><a href=\"{0}/\">{0}/</a></li>", dirName);
        }

        // Toon bestanden
        foreach (var file in files)
        {
            var fileName = Path.GetFileName(file);
            responseText.AppendFormat("<li><a href=\"{0}\">{0}</a></li>", fileName);
        }

        responseText.Append("</ul></body></html>");

        byte[] buffer = Encoding.UTF8.GetBytes(responseText.ToString());
        context.Response.StatusCode = (int)HttpStatusCode.OK;
        context.Response.ContentType = "text/html";
        context.Response.ContentLength64 = buffer.Length;
        context.Response.OutputStream.Write(buffer, 0, buffer.Length);
    }

    private void HandleOptions(HttpListenerContext context)
    {
        context.Response.AddHeader("Allow", "GET, OPTIONS, PROPFIND");
        context.Response.StatusCode = (int)HttpStatusCode.OK;
        context.Response.OutputStream.Close();
    }

    private void HandlePropFind(HttpListenerContext context)
    {
        var requestedUrl = context.Request.Url.AbsolutePath;
        var localPath = Path.Combine(_baseFolder, requestedUrl.TrimStart('/'));

        if (Directory.Exists(localPath))
        {
            SendDirectoryPropFindResponse(context, localPath);
        }
        else if (File.Exists(localPath))
        {
            SendFilePropFindResponse(context, localPath);
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            byte[] message = Encoding.UTF8.GetBytes("Bestand of map niet gevonden.");
            context.Response.OutputStream.Write(message, 0, message.Length);
        }
        context.Response.OutputStream.Close();
    }

    private void SendDirectoryPropFindResponse(HttpListenerContext context, string directoryPath)
    {
        var directories = Directory.GetDirectories(directoryPath);
        var files = Directory.GetFiles(directoryPath);

        var xmlResponse = new StringBuilder();
        xmlResponse.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
        xmlResponse.Append("<D:multistatus xmlns:D=\"DAV:\">");

        foreach (var directory in directories)
        {
            AppendDirectoryEntry(xmlResponse, directory);
        }

        foreach (var file in files)
        {
            AppendFileEntry(xmlResponse, file);
        }

        xmlResponse.Append("</D:multistatus>");

        byte[] buffer = Encoding.UTF8.GetBytes(xmlResponse.ToString());
        context.Response.StatusCode = (int)HttpStatusCode.OK;
        context.Response.ContentType = "application/xml";
        context.Response.ContentLength64 = buffer.Length;
        context.Response.OutputStream.Write(buffer, 0, buffer.Length);
    }

    private void AppendDirectoryEntry(StringBuilder xmlResponse, string directoryPath)
    {
        var dirName = Path.GetFileName(directoryPath);
        xmlResponse.Append("<D:response>");
        xmlResponse.AppendFormat("<D:href>{0}/</D:href>", dirName);
        xmlResponse.Append("<D:propstat>");
        xmlResponse.Append("<D:prop>");
        xmlResponse.Append("<D:resourcetype><D:collection/></D:resourcetype>");
        xmlResponse.Append("</D:prop>");
        xmlResponse.Append("<D:status>HTTP/1.1 200 OK</D:status>");
        xmlResponse.Append("</D:propstat>");
        xmlResponse.Append("</D:response>");
    }

    private void AppendFileEntry(StringBuilder xmlResponse, string filePath)
    {
        var fileName = Path.GetFileName(filePath);
        var fileInfo = new FileInfo(filePath);

        xmlResponse.Append("<D:response>");
        xmlResponse.AppendFormat("<D:href>{0}</D:href>", fileName);
        xmlResponse.Append("<D:propstat>");
        xmlResponse.Append("<D:prop>");
        xmlResponse.Append("<D:resourcetype/>");
        xmlResponse.AppendFormat("<D:getcontentlength>{0}</D:getcontentlength>", fileInfo.Length);
        xmlResponse.Append("</D:prop>");
        xmlResponse.Append("<D:status>HTTP/1.1 200 OK</D:status>");
        xmlResponse.Append("</D:propstat>");
        xmlResponse.Append("</D:response>");
    }

    private void HandleFileRequest(HttpListenerContext context, string filePath)
    {
        byte[] fileContent = File.ReadAllBytes(filePath);
        context.Response.StatusCode = (int)HttpStatusCode.OK;
        context.Response.ContentType = GetContentType(filePath);
        context.Response.ContentLength64 = fileContent.Length;
        context.Response.OutputStream.Write(fileContent, 0, fileContent.Length);
    }

    private string GetContentType(string filePath)
    {
        switch (Path.GetExtension(filePath).ToLower())
        {
            case ".html": return "text/html";
            case ".css": return "text/css";
            case ".js": return "application/javascript";
            case ".png": return "image/png";
            case ".jpg": return "image/jpeg";
            case ".gif": return "image/gif";
            default: return "application/octet-stream";
        }
    }

    // De ontbrekende methode voor bestanden
    private void SendFilePropFindResponse(HttpListenerContext context, string filePath)
    {
        var fileInfo = new FileInfo(filePath);

        var xmlResponse = new StringBuilder();
        xmlResponse.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
        xmlResponse.Append("<D:multistatus xmlns:D=\"DAV:\">");

        AppendFileEntry(xmlResponse, filePath);

        xmlResponse.Append("</D:multistatus>");

        byte[] buffer = Encoding.UTF8.GetBytes(xmlResponse.ToString());
        context.Response.StatusCode = (int)HttpStatusCode.OK;
        context.Response.ContentType = "application/xml";
        context.Response.ContentLength64 = buffer.Length;
        context.Response.OutputStream.Write(buffer, 0, buffer.Length);
    }


}



