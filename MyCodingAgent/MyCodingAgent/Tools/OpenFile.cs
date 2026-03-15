using MyCodingAgent.Helpers;
using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.Ollama;
using System.Text;

namespace MyCodingAgent.Tools;

public class OpenFile(Workspace workspace) : ITool
{
    public string Name
        => "open_file";
    public string Desciption
        => "opens a file and returns its content";
    public ToolParameter[] Parameters { get; } =
    [
        new ("path", "string", "path to the file to open")
    ];

    public async Task<ToolResult> Invoke(OllamaResponseMessageToolCallFunctionArguments toolArguments)
    {
        if (toolArguments.path == null)
            return new ToolResult(
                "parameter path is not supplied.",
                "parameter path is not supplied.",
                true);

        var file = workspace.GetFile(toolArguments.path);
        if (file == null)
        {
            return new ToolResult(
                $"Error opening file '{toolArguments.path}': file not found",
                $"Error opening file '{toolArguments.path}': file not found",
                true);
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine(file.RelativePath);
        var fileContent = await file.GetFileContent();
        foreach (var line in fileContent.GetLines())
        {
            sb.AppendLine($"{line.lineNumber,3}|{line.content}");
        }

        return new ToolResult(
            sb.ToString(),
            $"Showed file '{toolArguments.path}'",
            false);
    }
}