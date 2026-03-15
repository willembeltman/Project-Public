using MyCodingAgent.Helpers;
using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.Ollama;
using System.Text;
using static System.Net.WebRequestMethods;

namespace MyCodingAgent.Tools;

public class Find(Workspace workspace) : ITool
{
    public string Name 
        => "find";
    public string Desciption
        => "search for a specific string inside all files, result will be in next message";
    public ToolParameter[] Parameters { get; } =
    [
        new ("searchText", "string", "the specific string")
    ];

    public async Task<ToolResult> Invoke(OllamaResponseMessageToolCallFunctionArguments toolArguments)
    {
        if (toolArguments.searchText == null)
            return new ToolResult(
                "parameter searchText is not supplied.",
                "parameter searchText is not supplied.",
                true);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"searchText: '{toolArguments.searchText}'");
        foreach (var file in workspace.Files)
        {
            var fileContent = await file.GetFileContent();
            foreach (var line in fileContent.GetLines())
            {
                if (!line.content.Contains(toolArguments.searchText))
                    continue;

                sb.AppendLine($"{file.RelativePath}:{line.lineNumber} {line.content}");
            }
        }

        return new ToolResult(
            sb.ToString(),
            $"Showed search results",
            false);
    }
}
