using MyCodingAgent.Compile;
using MyCodingAgent.Models;
using System.Text;

namespace MyCodingAgent.Helpers;

public class PromptHelper(Workspace workspace)
{
    //public Task LastResponse(StringBuilder sb)
    //{
    //    if (workspace.AgentResponseResults.Count > 0)
    //    {
    //        var agentResponseResults = workspace.AgentResponseResults
    //            .OrderByDescending(a => a.Response.created_at)
    //            .Take(1)
    //            .ToArray();

    //        if (agentResponseResults.Length == 1)
    //            sb.AppendLine($"YOUR LAST RESPONSE / HISTORY");
    //        else
    //            sb.AppendLine($"YOUR LAST {agentResponseResults.Length} RESPONSES / HISTORY");

    //        sb.AppendLine();

    //        foreach (var result in agentResponseResults)
    //        {
    //            sb.AppendLine($"YOU AT {result.Response.created_at}");

    //            foreach (var line in result.Response.message.content.GetLines())
    //            {
    //                sb.AppendLine($"{line.lineNumber,3}|{line.content}");
    //            }

    //            if (result.ParseError != null)
    //                sb.AppendLine($"Parsing result: {result.ParseError}");

    //            if (result.ToolResults != null && result.ToolResults.Any())
    //            {
    //                sb.AppendLine($"Your parsed actions:");
    //                int i = 1;
    //                foreach (var action in result.ToolResults)
    //                {
    //                    sb.AppendLine($"{i}. Action: '{action.agentAction.name}' Result: '{action.result}'");
    //                    i++;
    //                }
    //            }
    //            else
    //            {
    //                sb.AppendLine($"Parsing result: Error, no actions found in response.");
    //            }
    //            sb.AppendLine();
    //        }
    //    }
    //    return Task.CompletedTask;
    //}
    //public async Task ProjectFiles(StringBuilder sb)
    //{
    //    sb.AppendLine("ALL PROJECT FILES");
    //    sb.AppendLine();

    //    if (workspace.Files.Count > 0)
    //    {
    //        foreach (var file in workspace.Files)
    //        {
    //            var fileContent = await file.GetFileContent();
    //            sb.AppendLine($"{file.RelativePath} ({fileContent.GetLineCount()} lines)");
    //        }
    //    }
    //    else
    //    {
    //        sb.AppendLine("<No files found in project>");
    //    }

    //    sb.AppendLine();
    //}
    //public async Task CurrentOpenFile(StringBuilder sb)
    //{
    //    sb.AppendLine("CURRENT OPENED FILE");
    //    sb.AppendLine();

    //    var currentOpenFile = (WorkspaceFile?)null;
    //    if (workspace.CurrentOpenFile != null)
    //        currentOpenFile = workspace.GetFile(workspace.CurrentOpenFile);
    //    //if (currentOpenFile == null)
    //    //    currentOpenFile = workspace.Files.FirstOrDefault();
    //    if (currentOpenFile != null)
    //    {
    //        sb.AppendLine(currentOpenFile.RelativePath);
    //        var fileContent = await currentOpenFile.GetFileContent();
    //        foreach (var line in fileContent.GetLines())
    //        {
    //            sb.AppendLine($"{line.lineNumber,3}|{line.content}");
    //        }
    //        sb.AppendLine();
    //    }
    //    else
    //    {
    //        sb.AppendLine("<No file opened>");
    //        sb.AppendLine();
    //    }
    //}
    //public async Task SearchResults(StringBuilder sb)
    //{
    //    sb.AppendLine("YOUR LAST SEARCH RESULTS");
    //    sb.AppendLine();

    //    if (!string.IsNullOrWhiteSpace(workspace.SearchText))
    //    {
    //        var searchResults = await workspace.GetSearchResults();
    //        sb.AppendLine($"searchText: '{workspace.SearchText}'");

    //        foreach (var r in searchResults)
    //        {
    //            sb.AppendLine($"{r.path}:{r.lineNumber} {r.line}");
    //        }

    //        sb.AppendLine();
    //    }
    //    else
    //    {
    //        sb.AppendLine("<No searchText text supplied>");
    //        sb.AppendLine();
    //    }
    //}
    public async Task CurrentPrompt(StringBuilder sb)
    {
        sb.AppendLine("USER REQUEST / ORIGINAL PROMPT");
        sb.AppendLine();
        sb.AppendLine(workspace.UserPrompt);
        sb.AppendLine();
    }
    //public Task SavedTasks(StringBuilder sb)
    //{
    //    sb.AppendLine("YOUR SAVED TASKS");
    //    sb.AppendLine();

    //    if (workspace.Tasks.Count > 0)
    //    {
    //        foreach (var t in workspace.Tasks)
    //        {
    //            sb.AppendLine($"TASK '{t.Id}': {t.Content}");
    //        }
    //    }
    //    else
    //    {
    //        sb.AppendLine("<No saved tasks>");
    //        sb.AppendLine();
    //    }

    //    return Task.CompletedTask;
    //}
    public async Task ShowErrorFiles(CompileResult compileResult, StringBuilder sb)
    {
        sb.AppendLine($"ERRORS (GROUPED BY FILES)");
        sb.AppendLine();
        foreach (var fileGroup in compileResult.Errors.GroupBy(a => a.File))
        {
            var relativePath = fileGroup.Key;
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                sb.AppendLine("FILE: <null>");
                foreach (var error in fileGroup)
                {
                    sb.AppendLine(error.FullError.TrimEnd('\n').TrimEnd('\r'));
                }
                sb.AppendLine();
                continue;
            }

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
