using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.Ollama;
using System.Reflection.Metadata;
using static System.Net.WebRequestMethods;

namespace MyCodingAgent.Tools;

public class CreateOrUpdateFile(Workspace workspace) : ITool
{
    public string Name
        => "create_or_update_file";
    public string Desciption
        => "creates a new file or overwrites an existing file with the provided content";
    public ToolParameter[] Parameters { get; } =
    [
        new ("path", "string", "path to the file"),
        new ("content", "string", "full content of the file")
    ];
    public async Task<ToolResult> Invoke(OllamaResponseMessageToolCallFunctionArguments toolArguments)
    {
        if (toolArguments.path == null)
            return new ToolResult(
                "parameter path is not supplied.",
                "parameter path is not supplied.",
                true);
        if (toolArguments.content == null)
            return new ToolResult(
                "parameter content is not supplied.",
                "parameter content is not supplied.",
                true);

        workspace.TryParseFullPath(toolArguments.path, out var fullPath);

        try
        {
            var file = workspace.GetFile(toolArguments.path);
            if (file == null)
            {
                var newFile = new WorkspaceFile(toolArguments.path, fullPath);
                await newFile.UpdateContent(toolArguments.content);
                workspace.Files.Add(newFile);
                return new ToolResult(
                    $"Created {toolArguments.path}",
                    $"Created {toolArguments.path}",
                    false);
            }
            else
            {
                await file.UpdateContent(toolArguments.content);
                return new ToolResult(
                    $"Updated {toolArguments.path}",
                    $"Updated {toolArguments.path}",
                    false);
            }
        }
        catch (Exception ex)
        {
            return new ToolResult(
                $"Error while updating '{toolArguments.path}': {ex.Message}",
                $"Error while updating",
                true);
        }
    }
}