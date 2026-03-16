using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using System.Text.RegularExpressions;

namespace MyCodingAgent.ToolCalls;

public class SearchAndReplaceAllFiles(Workspace workspace) : ITool
{
    public string Name
    => "search_and_replace_all_files";

    public string Description
        => "A global refactoring tool that replaces every occurrence of a string across all files in the workspace. Use this with extreme caution for project-wide renames (e.g., namespaces or shared constants). Always verify the search string is unique enough to avoid accidental changes in unrelated files.";

    public ToolParameter[] Parameters { get; } =
    [
        new ("searchText", "string", "The exact, case-sensitive string to find. Ensure this is specific enough to avoid false positives."),
        new ("replaceText", "string", "The exact, case-sensitive string to insert as a replacement.")
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