namespace MyCodingAgent.Ollama;

public record OllamaPromptMessage(
    string role,
    string content);
