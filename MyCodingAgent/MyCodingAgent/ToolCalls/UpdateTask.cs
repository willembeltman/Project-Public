using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.Ollama;

namespace MyCodingAgent.ToolCalls;

public class UpdateTask(Workspace workspace) : ITool
{
    public string Name
        => "update_task";
    public string Desciption
        => "overwrites a specific line range inside a task";
    public ToolParameter[] Parameters { get; } =
    [
        new ("id", "number", "id to the task"),
        new ("content", "string", "replacement content for the task")
    ]; 
    
    public async Task<ToolResult> Invoke(OllamaToolCallFunctionArguments toolArguments)
    {
        if (toolArguments.id == null)
            return new ToolResult(
                "parameter id is not supplied.",
                "parameter id is not supplied.",
                true);
        if (toolArguments.content == null)
            return new ToolResult(
                "parameter content is not supplied.",
                "parameter content is not supplied.",
                true);

        var task = workspace.GetTask(toolArguments.id.Value);
        if (task == null)
            return new ToolResult(
                $"Error could not find task {toolArguments.id}",
                $"Error could not find task",
                true);

        workspace.Tasks.Remove(task);
        var newTask = new WorkspaceTask(toolArguments.id.Value, toolArguments.content);
        workspace.Tasks.Add(newTask);
        return new ToolResult(
            $"Updated task '{toolArguments.id}'",
            $"Updated task",
            false);
    }
}