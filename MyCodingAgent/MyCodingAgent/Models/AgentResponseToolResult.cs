using MyCodingAgent.Ollama;

namespace MyCodingAgent.Models;

public record AgentResponseToolResult(
    OllamaResponseMessageToolCallFunction function,
    ToolResult result);
