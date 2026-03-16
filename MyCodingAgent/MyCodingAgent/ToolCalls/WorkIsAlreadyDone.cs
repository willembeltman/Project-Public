using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.Ollama;

namespace MyCodingAgent.ToolCalls;

public class WorkIsAlreadyDone(Workspace workspace) : ITool
{
    public string Name
        => "work_is_already_done";
    public string Desciption
        => "indicate all work is is already done, all user prompts are satisfied";
    public ToolParameter[] Parameters { get; } = [];
    public async Task<ToolResult> Invoke(OllamaToolCallFunctionArguments toolArguments)
    {
        workspace.PlanningIsDone = true;
        workspace.WorkIsDone = true;
        await workspace.Save();
        return new ToolResult("OK DONE!", "OK DONE!", false);
    }
}
