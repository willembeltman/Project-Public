using MyCodingAgent.Ollama;

namespace MyCodingAgent.Models;

public record AgentResponseToolResult(
    OllamaResponseMessageToolCallFunction toolCallFunction,
    ToolResult result);
