namespace MyCodingAgent;

public record AgentResponse(
    DateTime date,
    string responseText,
    string? thinkingText)
{
    public Line[] GetLines()
    {
        var lines = responseText.Split('\n');
        return Enumerable.Range(0, lines.Length)
            .Select(i => new Line(i, lines[i]))
            .ToArray();
    }
    public int GetLineCount() => GetLines().Length;
}