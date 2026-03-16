using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using System.Text.RegularExpressions;

namespace MyCodingAgent.ToolCalls;

public class SearchAndReplace(Workspace workspace) : ITool
{
    public string Name
        => "search_and_replace";
    public string Description
        => "Performs a precise find-and-replace operation within a single file. Both search and replacement are case-sensitive.";
    public ToolParameter[] Parameters { get; } =
    [
        new ("path", "string", "Path to the target file."),
        new ("searchText", "string", "The exact string to find."),
        new ("replaceText", "string", "The string to replace it with.")
    ];

    public async Task<ToolResult> Invoke(OllamaToolCall toolCall)
    {
        var toolArguments = toolCall.function.arguments;
        if (toolArguments.path == null)
            return new ToolResult(
                "parameter path is not supplied.",
                "parameter path is not supplied.",
                true);
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

        var file = workspace.GetFile(toolArguments.path);
        if (file == null)
            return new ToolResult(
                $"Error could not find path '{toolArguments.path}'",
                $"Error could not find path '{toolArguments.path}'",
                true);

        var content = await file.GetFileContent();
        var fileChanges = Regex.Matches(content, Regex.Escape(toolArguments.searchText)).Count;
        content = content.Replace(toolArguments.searchText, toolArguments.replaceText);

        if (fileChanges > 0)
        {
            await file.UpdateContent(content);
        }

        return new ToolResult(
            $"Replaced {fileChanges} instances",
            $"Replaced {fileChanges} instances",
            false);
    }
}