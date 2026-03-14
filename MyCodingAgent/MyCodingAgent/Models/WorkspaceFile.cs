namespace MyCodingAgent.Models;

public class WorkspaceFile(
    string relativePath,
    string fullPath)
{
    public string RelativePath { get; set; } = relativePath;
    public string FullPath { get; set; } = fullPath;

    public Task<string> GetFileContent()
    {
        return File.ReadAllTextAsync(FullPath);
    }
    public Task UpdateContent(string content)
    {
        var fileInfo = new FileInfo(FullPath);
        if (fileInfo.Directory == null)
            throw new Exception($"Weird stuff, directory is empty? {fileInfo}");
        if (fileInfo.Directory.Exists == false)
            fileInfo.Directory.Create();

        return File.WriteAllTextAsync(FullPath, content);
    }
    public async Task UpdateContent(int startLineNr, int endLineNr, string newContent)
    {
        var fileContent = await GetFileContent();
        var lines = fileContent.Split('\n').ToList();
        var newLines = newContent.Split('\n');

        lines.RemoveRange(startLineNr - 1, endLineNr - startLineNr + 1);
        lines.InsertRange(startLineNr - 1, newLines);

        var content = string.Join("\n", lines);
        await File.WriteAllTextAsync(FullPath, content);
    }
    public bool Exists()
    {
        return File.Exists(FullPath);
    }
    public void Delete()
    {
        File.Delete(FullPath);
    }
    public void Move(string newPath, string newFullPath)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(newFullPath)!);
        File.Move(FullPath, newFullPath, true);
        RelativePath = newPath;
        FullPath = newFullPath;
    }
}
