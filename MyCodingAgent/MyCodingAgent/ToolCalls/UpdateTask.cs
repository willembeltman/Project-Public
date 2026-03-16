using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;

namespace MyCodingAgent.ToolCalls;

public class UpdateSubTask(Workspace workspace) : ITool
{
    public string Name
        => "update_subTask";
    public string Desciption
        => "overwrites a specific line range inside a subTask";
    public ToolParameter[] Parameters { get; } =
    [
        new ("id", "number", "id to the subTask"),
        new ("content", "string", "replacement content for the subTask")
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

        var subTask = workspace.GetSubTask(toolArguments.id.Value);
        if (subTask == null)
            return new ToolResult(
                $"Error could not find subTask {toolArguments.id}",
                $"Error could not find subTask",
                true);

        workspace.SubTasks.Remove(subTask);
        var newSubTask = new WorkspaceSubTask(toolArguments.id.Value, toolArguments.content);
        workspace.SubTasks.Add(newSubTask);
        return new ToolResult(
            $"Updated subTask '{toolArguments.id}'",
            $"Updated subTask",
            false);
    }
}