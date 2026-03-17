using MyCodingAgent.Helpers;
using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using System.Text;
using System.Text.RegularExpressions;

namespace MyCodingAgent.ToolCalls;

public class WorkspaceReadonly_Tool(Workspace workspace) : IToolCall
{
    public string Name => "workspace";
    public string Description => "Interact with the workspace.";

    public ToolParameter[] Parameters { get; } =
    [
        new ("action", "string", "Action to perform", ["files_list", "read", "text_search", "compile"]),
        new ("path", "string", "The relative path from the workspace root (for all actions except 'files_list')", null, true),
        new ("query", "string", "Exact text to find (for 'text_search' and 'text_search_and_replace' action)", null, true),
    ];

    public async Task<ToolResult> Invoke(OllamaToolCall toolCall)
    {
        var toolArguments = toolCall.function.arguments;
        if (toolArguments.action == null)
            return new ToolResult(
                "Error parameter action is not supplied.",
                "Error parameter action is not supplied.",
                true);

        return toolArguments.action.ToLower() switch
        {
            "files_list" => await FilesList(toolCall),
            "read" => await Read(toolCall),
            "text_search" => await TextSearch(toolCall),
            "compile" => await Compile(toolCall),
            _ => new ToolResult(
                $"Error could not find action '{toolArguments.action}'",
                $"Error could not find action '{toolArguments.action}'",
                true)
        };
    }

    public async Task<ToolResult> FilesList(OllamaToolCall toolCall)
    {
        var toolArguments = toolCall.function.arguments;
        var listAllFilesText = await workspace.GetListAllFilesText();
        return new ToolResult(listAllFilesText, "Shown all files", false);
    }
    public async Task<ToolResult> Read(OllamaToolCall toolCall)
    {
        var toolArguments = toolCall.function.arguments;
        if (toolArguments.path == null)
            return new ToolResult(
                "Error parameter path is not supplied.",
                "Error parameter path is not supplied.",
                true);

        var file = workspace.GetFile(toolArguments.path);
        if (file == null)
        {
            return new ToolResult(
                $"Error opening file '{toolArguments.path}': file not found",
                $"Error opening file '{toolArguments.path}': file not found",
                true);
        }

        var fileContent = await file.GetFileContent();
        return new ToolResult(
            fileContent,
            $"Showed file '{toolArguments.path}'",
            false);
    }
    public async Task<ToolResult> TextSearch(OllamaToolCall toolCall)
    {
        var toolArguments = toolCall.function.arguments;
        if (toolArguments.query == null)
            return new ToolResult(
                "Error parameter query is not supplied.",
                "Error parameter query is not supplied.",
                true);
        var files = workspace.Files;
        if (string.IsNullOrEmpty(toolArguments.path) == false)
        {
            var file = workspace.GetFile(toolArguments.path);
            if (file != null)
            {
                files = [file];
            }
            else
            {
                return new ToolResult(
                    $"Error could not find file '{toolArguments.path}'",
                    $"Error could not find file",
                    true);
            }
        }

        StringBuilder sb = new StringBuilder();
        var found = 0;
        sb.AppendLine($"query: '{toolArguments.query}'");
        foreach (var file in files)
        {
            var fileContent = await file.GetFileContent();
            foreach (var line in fileContent.GetLines())
            {
                var index = line.content.ToLower().IndexOf(toolArguments.query.ToLower());
                if (index < 0)
                    continue;

                sb.AppendLine($"{file.RelativePath}:{line.lineNumber} {line.content}");
                found++;
            }
        }
        sb.AppendLine($"Found {found} instances.");

        return new ToolResult(
            sb.ToString(),
            $"Showed search results",
            false);
    }
    public async Task<ToolResult> Compile(OllamaToolCall toolCall)
    {
        var toolArguments = toolCall.function.arguments;
        var compileResult = await workspace.Compile(toolArguments.path);

        return new ToolResult(
            compileResult.Content,
            compileResult.Errors.Count > 0 ? "Compiled with error(s)" : compileResult.Errors.Count > 0 ? "Compiled with warning(s)" : "Compiled succesfully",
            false);
    }
}