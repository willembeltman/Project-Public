using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;

namespace MyCodingAgent.ToolCalls;

public class ShowAllSubTasks(Workspace workspace) : ITool
{
    public string Name
    => "show_all_subtasks";

    public string Description
        => "Returns a complete overview of the current plan, including all sub-tasks, their status, and unique IDs. Use this to track progress, identify remaining work, or before making changes to the task list.";
    public ToolParameter[] Parameters { get; } = [];
    public async Task<ToolResult> Invoke(OllamaToolCallFunctionArguments toolArguments)
    {
        var listAllSubTasksText = await workspace.GetListAllSubTasksText();
        return new ToolResult(listAllSubTasksText, "Shown all subtasks", false);
    }
}
