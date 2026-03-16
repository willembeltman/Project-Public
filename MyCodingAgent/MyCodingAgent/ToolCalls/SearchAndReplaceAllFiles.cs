using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using System.Text.RegularExpressions;

namespace MyCodingAgent.ToolCalls;

public class SearchAndReplaceAllFiles(Workspace workspace) : ITool
{
    public string Name
        => "search_and_replace_all_files";
    public string Desciption
        => "search for the search string inside all files of the workspace and replace all instances with the replacement string";
    public ToolParameter[] Parameters { get; } =
    [
        new ("searchText", "string", "the search string, is case-sensitive"),
        new ("replaceText", "string", "the replacement string, is case-sensitive")
    ];

    public async Task<ToolResult> Invoke(OllamaToolCallFunctionArguments toolArguments)
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