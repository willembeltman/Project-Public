using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;

namespace MyCodingAgent.ToolCalls;

public class CreateTask(Workspace workspace) : ITool
{
    public string Name
        => "create_task";
    public string Desciption
        => "create a new task with the provided content";
    public ToolParameter[] Parameters { get; } =
    [
        new ("content", "string", "full content of the task")
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
            var id = workspace.Tasks.Any() ? workspace.Tasks.Max(a => a.Id) : 0;
            var newTask = new WorkspaceTask(++id, toolArguments.content);
            workspace.Tasks.Add(newTask);
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