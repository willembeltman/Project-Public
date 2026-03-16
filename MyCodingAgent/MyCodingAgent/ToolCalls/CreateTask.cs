using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;

namespace MyCodingAgent.ToolCalls;

public class CreateSubTask(Workspace workspace) : ITool
{
    public string Name
    => "create_subtask";

    public string Description
        => "Strategic tool for the Planning Agent to decompose a complex objective into smaller, manageable tasks. Each sub-task should contain enough technical detail for a Coding Agent to execute it independently.";

    public ToolParameter[] Parameters { get; } =
    [
        new ("title", "string", "A short, descriptive name for the task (e.g., 'Update Auth Logic')."),
    new ("description", "string", "Detailed instructions or technical requirements. Define 'what' needs to be done and 'where' (e.g., 'Modify AuthService.cs to include JWT validation')."),
    new ("success_criteria", "string", "Specific conditions that must be met to consider this task finished (e.g., 'Compilation successful and unit tests pass').")
    ];
    public async Task<ToolResult> Invoke(OllamaToolCallFunctionArguments toolArguments)
    {
        if (toolArguments.content == null)
            return new ToolResult(
                "parameter content is not supplied.",
                "parameter content is not supplied.",
                true);
        try
        {
            var id = workspace.SubTasks.Any() ? workspace.SubTasks.Max(a => a.Id) : 0;
            var newSubTask = new WorkspaceSubTask(++id, toolArguments.content);
            workspace.SubTasks.Add(newSubTask);
            return new ToolResult(
                $"Created {toolArguments.id}",
                $"Created {toolArguments.id}",
                false);
        }
        catch (Exception ex)
        {
            return new ToolResult(
                $"Error while updating '{toolArguments.id}': {ex.Message}",
                $"Error while updating",
                true);
        }
    }
}