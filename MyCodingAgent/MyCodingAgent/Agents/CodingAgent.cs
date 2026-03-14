using MyCodingAgent.Compile;
using MyCodingAgent.Models;
using MyCodingAgent.Helpers;
using System.Text;
using MyCodingAgent.BaseAgents;
using MyCodingAgent.Interfaces;

namespace MyCodingAgent.Agents;

public class CodingAgent(Workspace workspace) : FencedBaseAgent(workspace), IAgent
{
    public async Task<string> GeneratePrompt(CompileResult compileResult)
    {
        var sb = new StringBuilder();
        await LastResponse(sb);
        await ProjectFiles(sb);
        await CurrentOpenFile(sb);
        await SearchResults(sb);
        await CurrentPrompt(sb);
        await SavedTasks(sb);

        var workspaceText = sb.ToString();
        var actionsText = GetActionsText();

        return $@"You are an autonomous software engineering agent operating inside a .NET 10 development workspace.
{actionsText}

{workspaceText}

WORKFLOW

1. Understand the request
2. Inspect files if needed
3. Make minimal edits
4. Verify using search or open_file

Always prefer small incremental steps.";
    }
    private Task LastResponse(StringBuilder sb)
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
    private async Task ProjectFiles(StringBuilder sb)
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
    private async Task CurrentOpenFile(StringBuilder sb)
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
    private async Task SearchResults(StringBuilder sb)
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
    private async Task CurrentPrompt(StringBuilder sb)
    {
        sb.AppendLine("USER REQUEST / ORIGINAL PROMPT");
        sb.AppendLine();
        sb.AppendLine(workspace.UserPrompt);
        sb.AppendLine();
    }
    private Task SavedTasks(StringBuilder sb)
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
}