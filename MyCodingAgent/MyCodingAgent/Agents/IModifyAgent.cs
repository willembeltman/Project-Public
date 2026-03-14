using MyCodingAgent.Compile;
using MyCodingAgent.Models;

namespace MyCodingAgent.Agents;

public interface IModifyAgent
{
    Task<string> GeneratePrompt(CompileResult compileResult);
    Task<bool> ProcessResponse(AgentResponse agentResponse);
}
