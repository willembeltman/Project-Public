using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;

namespace MyCodingAgent.ToolCalls;

public class ShowAllSubTasks(Workspace workspace) : ITool
{
    public string Name
        => "show_all_subTasks";
    public string Desciption
        => "retrieve a list of all subTasks inside the workspace";
    public ToolParameter[] Parameters { get; } = [];
    public async Task<ToolResult> Invoke(OllamaToolCallFunctionArguments toolArguments)
    {
        var listAllSubTasksText = await workspace.GetListAllSubTasksText();
        return new ToolResult(listAllSubTasksText, "Shown all subTasks", false);
    }
}
