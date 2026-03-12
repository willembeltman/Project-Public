namespace MyCodingAgent;

public class AgentAction
{
    public string Type { get; set; } = "";
    public string? Path { get; set; }
    public string? NewPath { get; set; }
    public string? Content { get; set; }
    public int? StartLine { get; set; }
    public int? EndLine { get; set; }
    public int? Id { get; set; }
}
