using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;

namespace MyCodingAgent.ToolCalls;

public class SubTaskIsFinished(Workspace workspace) : ITool
{
    public string Name
        => "subtask_is_finished";
    public string Description
        => "Marks a specific sub-task as completed. Only call this when all objectives for the task ID have been verified.";
    public ToolParameter[] Parameters { get; } =
    [
        new ("id", "string", "id to the subtask")
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
            var subtask = workspace.GetSubTask(toolArguments.id.Value);
            if (subtask != null)
            {
                workspace.SubTasks.Remove(subtask);
                return new ToolResult(
                    $"Deleted subtask {toolArguments.id}",
                    $"Deleted subtask",
                    false);
            }
            return new ToolResult(
                $"Error while deleting subtask '{toolArguments.id}': could not find subtask",
                $"Error while deleting subtask: could not find",
                true);
        }
        catch (Exception ex)
        {
            return new ToolResult(
                $"Error while deleting subtask '{toolArguments.id}': {ex.Message}",
                $"Error while deleting subtask",
                true);
        }
    }
}