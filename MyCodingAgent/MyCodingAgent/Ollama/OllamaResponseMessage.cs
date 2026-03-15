namespace MyCodingAgent.Ollama;

public record OllamaResponseMessage(
    string content,
    OllamaResponseMessageToolCall[]? tool_calls);
