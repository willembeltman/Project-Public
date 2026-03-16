using MyCodingAgent.Helpers;
using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using System.Text;
using static System.Net.WebRequestMethods;

namespace MyCodingAgent.ToolCalls;

public class Search(Workspace workspace) : ITool
{
    public string Name 
        => "search";
    public string Desciption
        => "show the results of a search for the specific string inside all files of the workspace";
    public ToolParameter[] Parameters { get; } =
    [
        new ("searchText", "string", "the specific string")
    ];

    public async Task<ToolResult> Invoke(OllamaToolCallFunctionArguments toolArguments)
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
