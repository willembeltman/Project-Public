using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;

namespace MyCodingAgent.ToolCalls;

public class CurrentSubTaskIsFinished_Tool(Workspace workspace) : IToolCall
{
    public string Name
        => "current_subtask_is_done";
    public string Description
        => "Marks the current sub-task as completed. Only call this when all objectives for the sub-task have been verified.";
    public ToolParameter[] Parameters { get; } =
    [
    ];

    public async Task<ToolResult> Invoke(OllamaToolCall toolCall)
    {
        var toolArguments = toolCall.function.arguments;

        var subtask = workspace.GetCurrentSubTask();
        if (subtask == null)
            throw new Exception("wtf?");

        try
        {
            if (subtask.Finished)
            {
                return new ToolResult(
                    $"Error subtask '{toolArguments.id}' already finished",
                    $"Error subtask already finished",
                    true);
            }

            subtask.Finished = true;
            workspace.CodingHistory.Clear();


            return new ToolResult(
                $"Finished subtask '{toolArguments.id}'",
                $"Finished subtask",
                false);
        }
        catch (Exception ex)
        {
            return new ToolResult(
                $"Error exception while finishing subtask '{toolArguments.id}': {ex.Message}",
                $"Error exception while finishing subtask ",
                true);
        }
    }
}