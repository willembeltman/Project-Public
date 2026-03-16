using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.Ollama;
using System.Text.RegularExpressions;

namespace MyCodingAgent.Tools;

public class FindAndReplace(Workspace workspace) : ITool
{
    public string Name
        => "find_and_replace";
    public string Desciption
        => "searches for a specific string inside given file and replaces it with the replacement string";
    public ToolParameter[] Parameters { get; } =
    [
        new ("path", "string", "path to the file to search in"),
        new ("searchText", "string", "the search string"),
        new ("replaceText", "string", "the replacement string")
    ];

    public async Task<ToolResult> Invoke(OllamaToolCallFunctionArguments toolArguments)
    {
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