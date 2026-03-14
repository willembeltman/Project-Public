using MyCodingAgent.Compile;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyCodingAgent.Agents;

public class CodingAgent(Workspace workspace) : IModifyAgent
{
    JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
    JsonSerializerOptions JsonDeserializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public string GeneratePrompt(CompileResult compileResult)
    {
        var sb = new StringBuilder();
        if (workspace.AgentResponseResults.Count > 0)
        {
            sb.AppendLine("YOUR LAST RESPONSE");
            sb.AppendLine();

            var agentResponseResults = workspace.AgentResponseResults
                .OrderByDescending(a => a.Response.date)
                .Take(1);
            foreach (var h in agentResponseResults)
            {
                foreach (var line in h.Response.responseText.GetLines())
                {
                    sb.AppendLine($"{line.lineNumber,3}|{line.content}");
                }
                sb.AppendLine();

                if (h.ParseError != null)
                    sb.AppendLine($"Parsing result: {h.ParseError}");
                else
                    sb.AppendLine("Parsing result: Succesfully parsed your response");

                if (h.Actions != null && h.Actions.Any())
                {
                    sb.AppendLine($"Your parsed actions:");
                    int i = 0;
                    foreach (var action in h.Actions)
                    {
                        i++;
                        sb.AppendLine($"{i}. Action: {action.AgentAction.type} '{action.AgentAction.path}' Result: {action.Result}");
                    }
                }
                sb.AppendLine();
            }
        }

        sb.AppendLine("USER REQUEST");
        sb.AppendLine();
        //foreach (var line in workspace.UserPrompt.GetLines())
        //{
        //    sb.AppendLine($"{line.lineNumber,3}|{line.content}");
        //}
        sb.AppendLine(workspace.UserPrompt);
        sb.AppendLine();

        var fileBrowser = GetWorkspaceFile();
        if (fileBrowser != null)
        {
            sb.AppendLine("CURRENT OPENED FILE");
            sb.AppendLine();
            sb.AppendLine(fileBrowser.RelativePath);
            sb.AppendLine();
            foreach (var line in fileBrowser.FileContent.GetLines())
            {
                sb.AppendLine($"{line.lineNumber,3}|{line.content}");
            }
            sb.AppendLine();
        }

        sb.AppendLine("ALL PROJECT FILES");
        sb.AppendLine();

        if (workspace.Files.Count > 0)
            foreach (var file in workspace.Files.Values)
                sb.AppendLine($"{file.RelativePath} ({file.FileContent.GetLineCount()} lines)");
        else
            sb.AppendLine("No files found in project");

        sb.AppendLine();

        sb.AppendLine("COMPILER OUTPUT");
        sb.AppendLine();

        if (!string.IsNullOrWhiteSpace(compileResult.Output))
        {
            foreach (var line in compileResult.Output.GetLines())
            {
                sb.AppendLine($"{line.lineNumber,3}|{line.content}");
            }
        }
        else
            sb.AppendLine("No compiler errors.");

        if (workspace.SearchText != null)
        {
            var searchResults = GetSearchResults();
            sb.AppendLine();
            sb.AppendLine("SEARCH RESULT");
            sb.AppendLine();
            sb.AppendLine($"Query: {workspace.SearchText}");
            sb.AppendLine($"Results: ");

            foreach (var r in searchResults)
            {
                sb.AppendLine($"{r.path,3}:{r.lineNumber} {r.line}");
            }
        }

        if (workspace.Tasks.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine("TASKS");
            sb.AppendLine();

            foreach (var t in workspace.Tasks)
            {
                sb.AppendLine($"{t.Key}. {t.Value}");
            }
        }

        var workspaceText = sb.ToString();

        return $@"You are an autonomous software engineering agent operating inside a .NET 10 development workspace.

You interact with this system through a JSON command protocol.

IMPORTANT RULES
1. Your response MUST be valid JSON.
2. The JSON MUST contain an array called ""actions"".
3. Do NOT include explanations outside the JSON.
4. Only use the actions listed below.
5. If you modify an existing file you MUST open it first.
6. Never assume file contents.
7. Always target .NET 10

AVAILABLE ACTIONS
search(content: string)
open_file(path: string)
create_or_update_file(path: string, content: string)
partial_overwrite_file(path: string, startLine: number, endLine: number, content: string)
delete_file(path: string)
move_file(path: string, newPath: string)

RESPONSE FORMAT
{{
  ""actions"": []
}}

EXAMPLE RESPONSE
User request:
Open Program.cs

Response:
{{
  ""actions"": [
    {{
      ""type"": ""open_file"",
      ""path"": ""Program.cs""
    }}
  ]
}}

{workspaceText}

Respond ONLY with JSON.

The first character of your response must be ""{{""
The last character must be ""}}""
Do not end response with ```";
    }
    private IEnumerable<AgentWorkspaceSearchResult> GetSearchResults()
    {
        if (workspace.SearchText == null)
            yield break;

        foreach (var file in workspace.Files.Values)
        {
            foreach (var line in file.FileContent.GetLines())
            {
                if (!line.content.Contains(workspace.SearchText))
                    continue;

                yield return new AgentWorkspaceSearchResult(
                        file.RelativePath,
                        line.lineNumber,
                        line.content);
            }
        }
    }
    private WorkspaceFile? GetWorkspaceFile()
    {
        WorkspaceFile? fileBrowser = null;

        if (workspace.CurrentOpenFile != null &&
            workspace.Files.TryGetValue(workspace.CurrentOpenFile, out var workspaceFile))
        {
            fileBrowser = workspaceFile;
        }

        if (fileBrowser == null)
        {
            var file = workspace.Files.Values.FirstOrDefault();
            if (file != null)
            {
                fileBrowser = file;
            }
        }

        return fileBrowser;
    }

    public async Task<bool> ProcessResponse(AgentResponse agentResponse)
    {
        (var agentActionCollection, var parseError) = TryParseActions(agentResponse.responseText);

        if (agentActionCollection == null || parseError != null)
        {
            workspace.AgentResponseResults.Add(new AgentResponseResult(agentResponse, parseError, []));
            return false;
        }

        var found = false;
        var list = new List<AgentActionResult>();
        foreach (var agentAction in agentActionCollection.Actions)
        {
            found = true;
            var result = (string?)null;
            switch (agentAction.type)
            {
                case "search":
                    result = await workspace.Search(agentAction.content!);
                    break;

                case "open_file":
                    result = await workspace.OpenFile(agentAction.path!);
                    break;

                case "create_or_update_file":
                    result = await workspace.CreateOrUpdateFile(agentAction.path!, agentAction.content!);
                    break;

                case "delete_file":
                    result = await workspace.DeleteFile(agentAction.path!);
                    break;

                case "move_file":
                    result = await workspace.MoveFile(agentAction.path!, agentAction.newPath!);
                    break;

                case "create_directory":
                    result = await workspace.CreateDirectory(agentAction.path!);
                    break;

                case "delete_directory":
                    result = await workspace.RemoveDirectory(agentAction.path!);
                    break;

                case "partial_overwrite_file":
                    result = await workspace.PartialOverwriteFile(
                        agentAction.path!,
                        agentAction.startLine!.Value,
                        agentAction.endLine!.Value,
                        agentAction.content!
                    );
                    break;

                case "create_or_update_task":
                    result = await workspace.CreateOrUpdateTask(agentAction.id!, agentAction.content!);
                    break;

                case "delete_task":
                    result = await workspace.DeleteTask(agentAction.id!);
                    break;

                default:
                    result = $"Action '{agentAction.type}' not found";
                    found = false;
                    break;
            }
            if (agentAction != null)
                list.Add(new AgentActionResult(agentAction, result));
        }
        workspace.AgentResponseResults.Add(new AgentResponseResult(agentResponse, null, [.. list]));
        await workspace.Save();
        return found;
    }
    private (AgentActionCollection? response, string? parseError) TryParseActions(string responseText)
    {
        try
        {
            var json = Clean(responseText);
            var newResponse = JsonSerializer.Deserialize<AgentActionCollection>(json, JsonDeserializerOptions);
            if (newResponse == null) return new(null, "Could not deserialize to { actions: [ ... ] }");
            return new(newResponse, null);
        }
        catch (Exception ex)
        {
            return new(null, ex.Message);
        }
    }
    private string Clean(string input)
    {
        input = input.Trim();

        if (input.StartsWith("```"))
        {
            var firstNewline = input.IndexOf('\n');
            var lastFence = input.LastIndexOf("```");

            if (firstNewline >= 0 && lastFence > firstNewline)
            {
                input = input.Substring(firstNewline + 1, lastFence - firstNewline - 1);
            }
        }

        return input.Trim();
    }
}