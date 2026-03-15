namespace MyCodingAgent.Ollama;

public record OllamaPrompt(
    OllamaMessage[] messages, 
    string tools);

public record OllamaMessage(
    string role,
    string content);

public enum OllamaAgentRole
{
    system, 
    user, 
    assistant, 
    tool
}