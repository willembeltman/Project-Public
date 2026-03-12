namespace MyCodingAgent;

public record WorkspaceFile(
    string RelativePath,
    string RealPath,
    string FileContent)
{
    public Line[] GetLines()
    {
        var lines = FileContent.Split('\n');
        return Enumerable.Range(0, lines.Length)
            .Select(i => new Line(i, lines[i]))
            .ToArray();
    }
    public int GetLineCount() => GetLines().Length;
}