using MyCodingAgent.Ollama;

namespace MyCodingAgent.Models;

public record ToolCallResult(
    OllamaToolCall tool_call,
    ToolResult result);
