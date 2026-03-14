namespace MyCodingAgent.Models;

public class AgentResponse
{
    public DateTime date { get; set; }
    public string responseText { get; set; } = string.Empty;
    public string? thinkingText { get; set; }
    public bool handled { get; set; }
}