using MyCodingAgent.Compile;
using MyCodingAgent.Models;
using MyCodingAgent.Ollama;

namespace MyCodingAgent.Interfaces;

public interface IAgent
{
    Task<OllamaPrompt> GeneratePrompt(CompileResult compileResult);
    Task<bool> ProcessResponse(OllamaPrompt prompt, AgentResponse agentResponse);
}
