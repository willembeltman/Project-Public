using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;

namespace MyCodingAgent.ToolCalls;

public class PlanningIsDone(Workspace workspace) : ITool
{
    public string Name
        => "planning_is_done";
    public string Desciption
        => "indicate all tasks has been planned, and coders can pick up the tasks";
    public ToolParameter[] Parameters { get; } = [];
    public async Task<ToolResult> Invoke(OllamaToolCallFunctionArguments toolArguments)
    {
        workspace.PlanningIsDone = true;
        await workspace.Save();
        return new ToolResult("OK DONE!", "OK DONE!", false);
    }
}
