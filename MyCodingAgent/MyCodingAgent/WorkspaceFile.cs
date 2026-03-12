namespace MyCodingAgent;

public record WorkspaceFile(
    string RelativePath,
    FileInfo FileInfo,
    string FileContent)
{
    public string[] Lines => FileContent.Split('\n');
    public int LineCount => Lines.Length;
}