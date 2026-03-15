namespace MyCodingAgent.Ollama;

public record OllamaResponseMessageToolCallFunction(
    string name,
    OllamaResponseMessageToolCallFunctionArguments arguments);