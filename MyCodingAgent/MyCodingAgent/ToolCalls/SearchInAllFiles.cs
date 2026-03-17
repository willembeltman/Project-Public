using MyCodingAgent.Helpers;
using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using System.Text;

namespace MyCodingAgent.ToolCalls;

public class SearchInAllFiles(Workspace workspace) : IToolCall
{
    public string Name
        => "search_in_all_files";
    public string Description
        => "Searches for a case-insensitive string across the entire workspace. Returns filenames and the context of matching lines.";
    public ToolParameter[] Parameters { get; } =
    [
        new ("searchText", "string", "The text to search for (case-insensitive).")
    ];

    public async Task<ToolResult> Invoke(OllamaToolCall toolCall)
    {
        var toolArguments = toolCall.function.arguments;
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
