using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;

namespace MyCodingAgent.ToolCalls;

public class UpdateFile(Workspace workspace) : ITool
{
    public string Name
        => "update_file";
    public string Description
        => "Updates a file by replacing a range of lines (inclusive). Use this for targeted code modifications.";
    public ToolParameter[] Parameters { get; } =
    [
        new ("path", "string", "Path to the file."),
        new ("startLine", "number", "The first line number to replace. (inclusive)"),
        new ("endLine", "number", "The last line number to replace. (inclusive)"),
        new ("content", "string", "The new code or text for this range.")
    ]; 
    
    public async Task<ToolResult> Invoke(OllamaToolCallFunctionArguments toolArguments)
    {
        if (toolArguments.path == null)
            return new ToolResult(
                "parameter path is not supplied.",
                "parameter path is not supplied.",
                true);
        if (toolArguments.startLine == null)
            return new ToolResult(
                "parameter startLine is not supplied.",
                "parameter startLine is not supplied.",
                true);
        if (toolArguments.endLine == null)
            return new ToolResult(
                "parameter endLine is not supplied.",
                "parameter endLine is not supplied.",
                true);
        if (toolArguments.content == null)
            return new ToolResult(
                "parameter content is not supplied.",
                "parameter content is not supplied.",
                true);

        var file = workspace.GetFile(toolArguments.path);
        if (file == null)
            return new ToolResult(
                $"Error could not find file '{toolArguments.path}'",
                $"Error could not find file",
                true);

        try
        {
            await file.UpdateContent(
                toolArguments.startLine.Value , 
                toolArguments.endLine.Value, 
                toolArguments.content);
            return new ToolResult(
                $"Updated file '{toolArguments.path}'",
                $"Updated file",
                false);
        }
        catch (Exception ex)
        {
            return new ToolResult(
                $"Error while updating file '{toolArguments.path}': {ex.Message}",
                $"Error while updating file",
                true);
        }
    }
}