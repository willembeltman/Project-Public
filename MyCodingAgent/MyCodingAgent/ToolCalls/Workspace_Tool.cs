using MyCodingAgent.Helpers;
using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using System.Text;
using System.Text.RegularExpressions;

namespace MyCodingAgent.ToolCalls;

public class Workspace_Tool(Workspace workspace) : IToolCall
{
    public string Name => "workspace";
    public string Description => "Interact with the workspace.";

    public ToolParameter[] Parameters { get; } =
    [
        new ("action", "string", "Action to perform", ["files_list", "read", "write", "append", "text_search", "text_search_and_replace", "delete", "move", "compile"]),
        new ("path", "string", "The relative path from the workspace root (for all actions except 'files_list', optional for compile)", null, true),
        new ("query", "string", "Exact text to find (for 'text_search' and 'text_search_and_replace' action)", null, true),
        new ("content", "string", "Content (for 'write', 'append' and 'text_search_and_replace' action)", null, true),
        new ("newPath", "string", "The new relative path from the workspace root (for 'move' action)", null, true),
        new ("lineNumber", "number", "Line number (optional for 'append' action)", null, true)
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
            "write" => await Write(toolCall),
            "text_search" => await TextSearch(toolCall),
            "text_search_and_replace" => await TextSearchAndReplace(toolCall),
            "delete" => await Delete(toolCall),
            "move" => await Move(toolCall),
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
    public async Task<ToolResult> Write(OllamaToolCall toolCall)
    {
        var toolArguments = toolCall.function.arguments;
        if (toolArguments.path == null)
            return new ToolResult(
                "Error parameter path is not supplied.",
                "Error parameter path is not supplied.",
                true);
        if (toolArguments.content == null)
            return new ToolResult(
                "Error parameter content is not supplied.",
                "Error parameter content is not supplied.",
                true);

        workspace.TryParseFullPath(toolArguments.path, out var fullPath);

        try
        {
            var file = workspace.GetFile(toolArguments.path);
            if (file == null)
            {
                var newFile = new WorkspaceFile(toolArguments.path, fullPath);
                await newFile.UpdateContent(toolArguments.content);
                workspace.Files.Add(newFile);
                return new ToolResult(
                    $"Created {toolArguments.path}",
                    $"Created {toolArguments.path}",
                    false);
            }
            else
            {
                await file.UpdateContent(toolArguments.content);
                return new ToolResult(
                    $"Updated {toolArguments.path}",
                    $"Updated {toolArguments.path}",
                    false);
            }
        }
        catch (Exception ex)
        {
            return new ToolResult(
                $"Error while updating '{toolArguments.path}': {ex.Message}",
                $"Error while updating",
                true);
        }
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
    public async Task<ToolResult> TextSearchAndReplace(OllamaToolCall toolCall)
    {
        var toolArguments = toolCall.function.arguments;
        if (toolArguments.path == null)
            return new ToolResult(
                "parameter path is not supplied.",
                "parameter path is not supplied.",
                true);
        if (toolArguments.query == null)
            return new ToolResult(
                "parameter query is not supplied.",
                "parameter query is not supplied.",
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
        var fileChanges = Regex.Matches(content, Regex.Escape(toolArguments.query)).Count;
        content = content.Replace(toolArguments.query, toolArguments.replaceText);

        if (fileChanges > 0)
        {
            await file.UpdateContent(content);
        }

        return new ToolResult(
            $"Replaced {fileChanges} instances",
            $"Replaced {fileChanges} instances",
            false);
    }
    public async Task<ToolResult> Delete(OllamaToolCall toolCall)
    {
        var toolArguments = toolCall.function.arguments;
        if (toolArguments.path == null)
            return new ToolResult(
                "Error parameter path is not supplied.",
                "Error parameter path is not supplied.",
                true);

        workspace.TryParseFullPath(toolArguments.path, out var fullPath);

        try
        {
            var file = workspace.GetFile(toolArguments.path);
            if (file != null)
            {
                file.Delete();
                workspace.Files.Remove(file);
                return new ToolResult(
                    $"Deleted file {toolArguments.path}",
                    $"Deleted file",
                    false);
            }
            return new ToolResult(
                $"Error while deleting file '{toolArguments.path}': could not find file",
                $"Error while deleting file: could not find",
                true);
        }
        catch (Exception ex)
        {
            return new ToolResult(
                $"Error while deleting file '{toolArguments.path}': {ex.Message}",
                $"Error while deleting file",
                true);
        }
    }
    public async Task<ToolResult> Move(OllamaToolCall toolCall)
    {
        var toolArguments = toolCall.function.arguments;
        if (toolArguments.path == null)
            return new ToolResult(
                "Error parameter path is not supplied.",
                "Error parameter path is not supplied.",
                true);
        if (toolArguments.newPath == null)
            return new ToolResult(
                "Error parameter newPath is not supplied.",
                "Error parameter newPath is not supplied.",
                true);

        workspace.TryParseFullPath(toolArguments.newPath, out var newFullPath);

        try
        {
            var file = workspace.GetFile(toolArguments.path);
            if (file != null && file.Exists())
            {
                file.Move(toolArguments.newPath, newFullPath);
                return new ToolResult(
                    $"Moved file {toolArguments.path} -> {toolArguments.newPath}",
                    $"Moved file",
                    false);
            }
            return new ToolResult(
                $"Error while moving file '{toolArguments.path}': could not find file",
                $"Error while moving file: could not find",
                true);
        }
        catch (Exception ex)
        {
            return new ToolResult(
                $"Error while moving file '{toolArguments.path}': {ex.Message}",
                $"Error while moving file",
                true);
        }
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