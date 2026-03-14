using MyCodingAgent.Compile;
using MyCodingAgent.Models;
using MyCodingAgent.Helpers;
using System.Text;
using MyCodingAgent.BaseAgents;
using MyCodingAgent.Interfaces;

namespace MyCodingAgent.Agents;

public class CodingAgent(Workspace workspace) : JsonAgent(workspace), IAgent
{
    public async Task<string> GeneratePrompt(CompileResult compileResult)
    {
        var sb = new StringBuilder();
        if (workspace.AgentResponseResults.Count > 0)
        {
            sb.AppendLine("YOUR LAST RESPONSE");
            sb.AppendLine();

            var agentResponseResults = workspace.AgentResponseResults
                .OrderByDescending(a => a.response.date)
                .Take(1);
            foreach (var h in agentResponseResults)
            {
                foreach (var line in h.response.responseText.GetLines())
                {
                    sb.AppendLine($"{line.lineNumber,3}|{line.content}");
                }
                sb.AppendLine();

                if (h.parseError != null)
                    sb.AppendLine($"Parsing result: {h.parseError}");
                else
                    sb.AppendLine("Parsing result: Succesfully parsed your response");

                if (h.actions != null && h.actions.Any())
                {
                    sb.AppendLine($"Your parsed actions:");
                    if (h.actions.Length > 0)
                    {
                        int i = 0;
                        foreach (var action in h.actions)
                        {
                            i++;
                            sb.AppendLine($"{i}. Action: {action.agentAction.type} '{action.agentAction.path ?? action.agentAction.searchText}' Result: {action.result}");
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

        sb.AppendLine("ALL PROJECT FILES");
        sb.AppendLine();

        if (workspace.Files.Count > 0)
            foreach (var file in workspace.Files)
            {
                var fileContent = await file.GetFileContent();
                sb.AppendLine($"{file.RelativePath} ({fileContent.GetLineCount()} lines)");
            }
        else
            sb.AppendLine("<No files found in project>");

        sb.AppendLine();

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
            sb.AppendLine();
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

        if (workspace.SearchText != null)
        {
            var searchResults = await workspace.GetSearchResults();
            sb.AppendLine("SEARCH RESULTS");
            sb.AppendLine();
            sb.AppendLine($"searchText: {workspace.SearchText}");
            sb.AppendLine($"Results: ");

            foreach (var r in searchResults)
            {
                sb.AppendLine($"{r.path,3}:{r.lineNumber} {r.line}");
            }

            sb.AppendLine();
        }

        sb.AppendLine("USER REQUEST / ORIGINAL PROMPT");
        sb.AppendLine();
        sb.AppendLine(workspace.UserPrompt);

        if (workspace.Tasks.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine("YOUR SAVED TASKS");
            sb.AppendLine();

            foreach (var t in workspace.Tasks)
            {
                sb.AppendLine($"{t.Id}. {t.Content}");
            }
        }

        var actionsText = GetActionsText();
        var workspaceText = sb.ToString();

        return $@"You are an autonomous software engineering agent operating inside a .NET 10 development workspace.

You interact with this system through a JSON command protocol.
{actionsText}

IMPORTANT RULES
1. Your response MUST be valid JSON.
2. The JSON MUST contain an array called ""actions"".
3. Do NOT include explanations outside the JSON.
4. Only use the actions listed above.
5. Never assume file contents, open the file first.
6. Target .NET 10
8. To prevent json parsing issues, please try to only update 1 row each action
9. Do not find_and_replace large textblocks

{workspaceText}

Respond ONLY with JSON.

The first character of your response must be ""{{""
The last character must be ""}}""
Do not end response with ```";
    }
}