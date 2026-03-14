using MyCodingAgent.Compile;
using MyCodingAgent.Models;

namespace MyCodingAgent.Interfaces;

public interface IAgent
{
    Task<string> GeneratePrompt(CompileResult compileResult);
    Task<bool> ProcessResponse(AgentResponse agentResponse);
}
