namespace MyCodingAgent.Ollama;

public record OllamaResponse(
    string model,
    DateTime created_at,
    OllamaResponseMessage message);
