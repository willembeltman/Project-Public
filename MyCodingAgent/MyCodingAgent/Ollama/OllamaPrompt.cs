using MyCodingAgent.Models;

namespace MyCodingAgent.Ollama;

public record OllamaPrompt(
    OllamaMessage[] messages,
    Tool[] tools);
