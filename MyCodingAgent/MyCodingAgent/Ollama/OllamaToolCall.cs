namespace MyCodingAgent.Ollama;

public record OllamaToolCall(
    string id,
    OllamaToolCallFunction function);
