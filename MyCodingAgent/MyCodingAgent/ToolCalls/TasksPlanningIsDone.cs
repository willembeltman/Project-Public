using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;

namespace MyCodingAgent.ToolCalls;

public class SubTasksPlanningIsDone(Workspace workspace) : ITool
{
    public string Name
        => "subTasks_planning_is_done";
    public string Desciption
        => "indicate all subTasks has been planned, and coders can pick up the subTasks";
    public ToolParameter[] Parameters { get; } = [];
    public async Task<ToolResult> Invoke(OllamaToolCallFunctionArguments toolArguments)
    {
        workspace.PlanningIsDone = true;
        await workspace.Save();
        return new ToolResult("OK DONE!", "OK DONE!", false);
    }
}
