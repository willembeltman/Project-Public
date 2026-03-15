using MyCodingAgent.Models;

namespace MyCodingAgent.Ollama;

public record OllamaPrompt(
    OllamaPromptMessage[] messages,
    Tool[] tools);
