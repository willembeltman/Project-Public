namespace MyCodingAgent;

public record AgentResponse(
    int index,
    string responseText, 
    string? thinkingText);
