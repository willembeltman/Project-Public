using MyCodingAgent.Ollama;

namespace MyCodingAgent.Models;

public record AgentResponseResult(
    OllamaPrompt Prompt,
    OllamaResponse Response,
    AgentResponseToolResult[] ToolResults);