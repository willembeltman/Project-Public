using MyCodingAgent.Models;

namespace MyCodingAgent.Interfaces;

public interface IAgent
{
    Task<OllamaPrompt> GeneratePrompt(CompileResult compileResult);
    Task<bool> ProcessResponse(OllamaPrompt prompt, OllamaResponse agentResponse);
}
