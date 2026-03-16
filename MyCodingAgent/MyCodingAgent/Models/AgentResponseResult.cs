using MyCodingAgent.Ollama;

namespace MyCodingAgent.Models;

public record AgentResponseResult(
    OllamaPrompt Prompt,
    OllamaResponse response,
    AgentResponseToolResult[] ToolResults);