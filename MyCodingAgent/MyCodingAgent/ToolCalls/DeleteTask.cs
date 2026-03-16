using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;

namespace MyCodingAgent.ToolCalls;

public class DeleteSubTask(Workspace workspace) : ITool
{
    public string Name
        => "delete_subTask";
    public string Desciption
        => "delete a subTask from the workspace";
    public ToolParameter[] Parameters { get; } =
    [
        new ("id", "string", "id to the subTask")
    ];

    public async Task<ToolResult> Invoke(OllamaToolCallFunctionArguments toolArguments)
    {
        if (toolArguments.id == null)
            return new ToolResult(
                "parameter id is not supplied.",
                "parameter id is not supplied.",
                true);

        try
        {
            var subTask = workspace.GetSubTask(toolArguments.id.Value);
            if (subTask != null)
            {
                workspace.SubTasks.Remove(subTask);
                return new ToolResult(
                    $"Deleted subTask {toolArguments.id}",
                    $"Deleted subTask",
                    false);
            }
            return new ToolResult(
                $"Error while deleting subTask '{toolArguments.id}': could not find subTask",
                $"Error while deleting subTask: could not find",
                true);
        }
        catch (Exception ex)
        {
            return new ToolResult(
                $"Error while deleting subTask '{toolArguments.id}': {ex.Message}",
                $"Error while deleting subTask",
                true);
        }
    }
}