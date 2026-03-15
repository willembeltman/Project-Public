using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.Ollama;
using static System.Net.WebRequestMethods;

namespace MyCodingAgent.Tools;

public class DeleteFile(Workspace workspace) : ITool
{
    public string Name
        => "delete_file";
    public string Desciption
        => "deletes a file";
    public ToolParameter[] Parameters { get; } =
    [
        new ("path", "string", "path to the file")
    ];

    public async Task<ToolResult> Invoke(OllamaResponseMessageToolCallFunctionArguments toolArguments)
    {
        if (toolArguments.path == null)
            return new ToolResult(
                "parameter path is not supplied.",
                "parameter path is not supplied.",
                true);

        workspace.TryParseFullPath(toolArguments.path, out var fullPath);

        try
        {
            var file = workspace.GetFile(toolArguments.path);
            if (file != null)
            {
                file.Delete();
                workspace.Files.Remove(file);
                return new ToolResult(
                    $"Deleted file {toolArguments.path}",
                    $"Deleted file",
                    false);
            }
            return new ToolResult(
                $"Error while deleting file '{toolArguments.path}': could not find file",
                $"Error while deleting file: could not find",
                true);
        }
        catch (Exception ex)
        {
            return new ToolResult(
                $"Error while deleting file '{toolArguments.path}': {ex.Message}",
                $"Error while deleting file",
                true);
        }
    }
}