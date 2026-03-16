namespace MyCodingAgent.Ollama;

public record OllamaResponse(
    string model,
    DateTime created_at,
    OllamaMessage message);
