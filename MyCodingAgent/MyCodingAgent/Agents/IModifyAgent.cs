using MyCodingAgent.Compile;

namespace MyCodingAgent.Agents;

public interface IModifyAgent
{
    string GeneratePrompt(CompileResult compileResult);
    Task<bool> ProcessResponse(AgentResponse agentResponse);
}
