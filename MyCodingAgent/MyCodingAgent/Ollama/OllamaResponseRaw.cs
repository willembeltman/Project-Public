namespace MyCodingAgent.Ollama;

public record OllamaResponseRaw(
    string? model,
    DateTime? created_at,
    string? thinking,
    string? response,
    bool done);
