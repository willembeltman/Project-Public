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
        var found = 0;
        sb.AppendLine($"searchText: '{toolArguments.searchText}'");
        foreach (var file in workspace.Files)
        {
            var fileContent = await file.GetFileContent();
            foreach (var line in fileContent.GetLines())
            {
                var index = line.content.ToLower().IndexOf(toolArguments.searchText.ToLower());
                if (index < 0)
                    continue;

                sb.AppendLine($"{file.RelativePath}:{line.lineNumber}:{index} {line.content}");
                found++;
            }
        }
        sb.AppendLine($"Found {found} instances.");

        return new ToolResult(
            sb.ToString(),
            $"Showed search results",
            false);
    }
}
