using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.Ollama;

namespace MyCodingAgent.ToolCalls;

public class UpdateFile(Workspace workspace) : ITool
{
    public string Name
        => "update_file";
    public string Desciption
        => "overwrites a specific line range inside a file";
    public ToolParameter[] Parameters { get; } =
    [
        new ("path", "string", "path to the file"),
        new ("startLine", "number", "first line to overwrite (inclusive)"),
        new ("endLine", "number", "last line to overwrite (inclusive)"),
        new ("content", "string", "replacement content for the specified line range")
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