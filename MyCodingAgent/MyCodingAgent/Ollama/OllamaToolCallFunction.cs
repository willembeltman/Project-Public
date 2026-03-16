namespace MyCodingAgent.Ollama;

public record OllamaToolCallFunction(
    int? index,
    string name,
    OllamaToolCallFunctionArguments arguments);