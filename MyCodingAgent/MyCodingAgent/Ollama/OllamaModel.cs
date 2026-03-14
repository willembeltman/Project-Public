
namespace MyCodingAgent.Ollama;

public record OllamaModel(
    string Name, 
    long? Size, 
    DateTime? LastModified);