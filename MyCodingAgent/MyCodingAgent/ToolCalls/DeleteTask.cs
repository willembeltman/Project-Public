using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;

namespace MyCodingAgent.ToolCalls;

public class DeleteTask(Workspace workspace) : ITool
{
    public string Name
        => "delete_task";
    public string Desciption
        => "deletes a task";
    public ToolParameter[] Parameters { get; } =
    [
        new ("id", "string", "id to the task")
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
            var task = workspace.GetTask(toolArguments.id.Value);
            if (task != null)
            {
                workspace.Tasks.Remove(task);
                return new ToolResult(
                    $"Deleted task {toolArguments.id}",
                    $"Deleted task",
                    false);
            }
            return new ToolResult(
                $"Error while deleting task '{toolArguments.id}': could not find task",
                $"Error while deleting task: could not find",
                true);
        }
        catch (Exception ex)
        {
            return new ToolResult(
                $"Error while deleting task '{toolArguments.id}': {ex.Message}",
                $"Error while deleting task",
                true);
        }
    }
}