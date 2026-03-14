namespace MyCodingAgent.Models;

public record AgentResponse(
    DateTime date,
    string responseText,
    string? thinkingText);