namespace MyCodingAgent;

public record AgentResponse(
    DateTime date,
    string responseText, 
    string? thinkingText);
