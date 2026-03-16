using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;

namespace MyCodingAgent.ToolCalls;

public class CreateSubTask(Workspace workspace) : ITool
{
    public string Name
        => "create_subTask";
    public string Desciption
        => "create a new subTask with the provided content";
    public ToolParameter[] Parameters { get; } =
    [
        new ("content", "string", "full content of the subTask")
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