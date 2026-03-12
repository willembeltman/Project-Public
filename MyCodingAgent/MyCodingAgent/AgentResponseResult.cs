namespace MyCodingAgent;

public record AgentResponseResult(
    AgentResponse Response,
    string? ParseError,
    AgentActionResult[] Actions);