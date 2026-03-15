using MyCodingAgent.Ollama;

namespace MyCodingAgent.Models;

public record AgentResponseResult(
    string request,
    AgentResponse response,
    string? parseError,
    AgentActionResult[] actionResults);