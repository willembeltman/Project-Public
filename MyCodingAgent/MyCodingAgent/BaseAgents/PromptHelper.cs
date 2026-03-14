using MyCodingAgent.Compile;
using MyCodingAgent.Helpers;
using MyCodingAgent.Models;
using System.Text;

namespace MyCodingAgent.BaseAgents;

public class PromptHelper(Workspace workspace)
{
    public Task LastResponse(StringBuilder sb)
    {
        if (workspace.AgentResponseResults.Count > 0)
        {
            sb.AppendLine("LAST 3 RESPONSES");
            sb.AppendLine();

            var agentResponseResults = workspace.AgentResponseResults
                .OrderByDescending(a => a.response.date)
                .Take(3);
            foreach (var result in agentResponseResults)
            {
                sb.AppendLine($"YOU @ {result.response.date}");

                foreach (var line in result.response.responseText.GetLines())
                {
                    sb.AppendLine($"{line.lineNumber,3}|{line.content}");
                }
                sb.AppendLine();

                if (result.parseError != null)
                    sb.AppendLine($"Parsing result: {result.parseError}");
                else
                    sb.AppendLine("Parsing result: Succesfully parsed your response");

                if (result.actions != null && result.actions.Any())
                {
                    sb.AppendLine($"Your parsed actions:");
                    if (result.actions.Length > 0)
                    {
                        int i = 0;
                        foreach (var action in result.actions)
                        {
                            sb.AppendLine($"{i}. Action: {action.agentAction.type} Result: {action.result}");
                            i++;
                        }
                    }
                    else
                    {
                        sb.AppendLine($"<No actions found in response>");
                    }
                }
                sb.AppendLine();
            }
        }
        return Task.CompletedTask;
    }
    public async Task ProjectFiles(StringBuilder sb)
    {
        sb.AppendLine("ALL PROJECT FILES");
        sb.AppendLine();

        if (workspace.Files.Count > 0)
        {
            foreach (var file in workspace.Files)
            {
                var fileContent = await file.GetFileContent();
                sb.AppendLine($"{file.RelativePath} ({fileContent.GetLineCount()} lines)");
            }
        }
        else
        {
            sb.AppendLine("<No files found in project>");
        }

        sb.AppendLine();
    }
    public async Task CurrentOpenFile(StringBuilder sb)
    {
        sb.AppendLine("CURRENT OPENED FILE");
        sb.AppendLine();

        var currentOpenFile = (WorkspaceFile?)null;
        if (workspace.CurrentOpenFile != null)
            currentOpenFile = workspace.GetFile(workspace.CurrentOpenFile);
        if (currentOpenFile == null)
            currentOpenFile = workspace.Files.FirstOrDefault();
        if (currentOpenFile != null)
        {
            sb.AppendLine(currentOpenFile.RelativePath);
            var fileContent = await currentOpenFile.GetFileContent();
            foreach (var line in fileContent.GetLines())
            {
                sb.AppendLine($"{line.lineNumber,3}|{line.content}");
            }
            sb.AppendLine();
        }
        else
        {
            sb.AppendLine("<No file opened>");
            sb.AppendLine();
        }
    }
    public async Task SearchResults(StringBuilder sb)
    {
        sb.AppendLine("SEARCH RESULTS");
        sb.AppendLine();

        if (!string.IsNullOrWhiteSpace(workspace.SearchText))
        {
            var searchResults = await workspace.GetSearchResults();
            sb.AppendLine($"SearchText: {workspace.SearchText}");
            sb.AppendLine($"Results: ");

            foreach (var r in searchResults)
            {
                sb.AppendLine($"{r.path}:{r.lineNumber} {r.line}");
            }

            sb.AppendLine();
        }
        else
        {
            sb.AppendLine("<No search text supplied>");
            sb.AppendLine();
        }
    }
    public async Task CurrentPrompt(StringBuilder sb)
    {
        sb.AppendLine("USER REQUEST / ORIGINAL PROMPT");
        sb.AppendLine();
        sb.AppendLine(workspace.UserPrompt);
        sb.AppendLine();
    }
    public Task SavedTasks(StringBuilder sb)
    {
        sb.AppendLine("YOUR SAVED TASKS");
        sb.AppendLine();

        if (workspace.Tasks.Count > 0)
        {
            foreach (var t in workspace.Tasks)
            {
                sb.AppendLine($"{t.Id}. {t.Content}");
            }
        }
        else
        {
            sb.AppendLine("<No saved tasks>");
            sb.AppendLine();
        }

        return Task.CompletedTask;
    }
    public async Task ShowErrorFiles(CompileResult compileResult, StringBuilder sb)
    {
        sb.AppendLine($"FILES WITH ERRORS");
        sb.AppendLine();
        foreach (var fileGroup in compileResult.Errors.GroupBy(a => a.File))
        {
            var relativePath = fileGroup.Key;
            if (string.IsNullOrWhiteSpace(relativePath)) continue;

            var file = workspace.GetFile(relativePath);
            if (file != null)
            {
                sb.AppendLine($"FILE: {relativePath}");
                sb.AppendLine("CODE");
                var fileContent = await file.GetFileContent();
                foreach (var line in fileContent.GetLines())
                {
                    sb.AppendLine($"{line.lineNumber,3}|{line.content}");
                }
                sb.AppendLine();
                sb.AppendLine("ERRORS");
                foreach (var error in fileGroup)
                {
                    sb.AppendLine(error.FullError.TrimEnd('\n').TrimEnd('\r'));
                }

                sb.AppendLine();
            }
        }
    }
}
