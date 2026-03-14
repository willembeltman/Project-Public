namespace MyCodingAgent.Models;

public record AgentResponseResult(
    AgentResponse response,
    string? parseError,
    AgentActionResult[] actions);