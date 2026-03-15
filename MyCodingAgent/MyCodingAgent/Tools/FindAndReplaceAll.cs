using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.Ollama;
using System.Text.RegularExpressions;

namespace MyCodingAgent.Tools;

public class FindAndReplaceAll(Workspace workspace) : ITool
{
    public string Name
        => "find_and_replace_all";
    public string Desciption
        => "searches for a specific string inside all files and replaces it with the replacement string";
    public ToolParameter[] Parameters { get; } =
    [
        new ("searchText", "string", "the search string"),
        new ("replaceText", "string", "the replacement string")
    ];

    public async Task<ToolResult> Invoke(OllamaResponseMessageToolCallFunctionArguments toolArguments)
    {
        if (toolArguments.searchText == null)
            return new ToolResult(
                "parameter searchText is not supplied.",
                "parameter searchText is not supplied.",
                true);
        if (toolArguments.replaceText == null)
            return new ToolResult(
                "parameter replaceText is not supplied.",
                "parameter replaceText is not supplied.",
                true);

        var allChanges = 0;
        foreach (var file in workspace.Files)
        {
            var content = await file.GetFileContent();
            var fileChanges = Regex.Matches(content, Regex.Escape(toolArguments.searchText)).Count;
            content = content.Replace(toolArguments.searchText, toolArguments.replaceText);

            if (fileChanges > 0)
            {
                await file.UpdateContent(content);
            }
            allChanges += fileChanges;
        }

        return new ToolResult(
            $"Replaced {allChanges} instances",
            $"Replaced {allChanges} instances",
            false);
    }
}