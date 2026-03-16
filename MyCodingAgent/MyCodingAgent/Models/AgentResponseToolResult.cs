using MyCodingAgent.Ollama;

namespace MyCodingAgent.Models;

public record AgentResponseToolResult(
    OllamaToolCall tool_call,
    ToolResult result);
