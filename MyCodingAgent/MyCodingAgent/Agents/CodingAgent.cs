using MyCodingAgent.Compile;
using MyCodingAgent.Models;
using MyCodingAgent.Helpers;
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
                    int i = 0;
                    foreach (var action in h.actions)
                    {
                        i++;
                        sb.AppendLine($"{i}. Action: {action.agentAction.type} '{action.agentAction.path}' Result: {action.result}");
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

        var fileBrowser = (WorkspaceFile?)null;
        if (workspace.CurrentOpenFile != null)
            fileBrowser = workspace.GetFile(workspace.CurrentOpenFile);
        if (fileBrowser == null)
            fileBrowser = workspace.Files.FirstOrDefault();
        if (fileBrowser != null)
        {
            sb.AppendLine("CURRENT OPENED FILE");
            sb.AppendLine();
            sb.AppendLine(fileBrowser.RelativePath);
            sb.AppendLine();
            var fileContent = await fileBrowser.GetFileContent();
            foreach (var line in fileContent.GetLines())
            {
                sb.AppendLine($"{line.lineNumber,3}|{line.content}");
            }
            sb.AppendLine();
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
            var searchResults = await GetSearchResults();
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
                sb.AppendLine($"{t.Id}. {t.Content}");
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
find(searchText: string)
find_and_replace(path: string, searchText: string, replaceText: string)
find_and_replace_all(searchText: string, replaceText: string)
open_file(path: string)
create_or_update_file(path: string, content: string)
partial_overwrite_file(path: string, startLine: number, endLine: number, content: string)
move_file(path: string, newPath: string)
delete_file(path: string)
create_or_update_task(path: string, content: string)
delete_task(path: string)

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
    private async Task<SearchResult[]> GetSearchResults()
    {
        if (workspace.SearchText == null)
            return [];

        var list = new List<SearchResult>();
        foreach (var file in workspace.Files)
        {
            var fileContent = await file.GetFileContent();
            foreach (var line in fileContent.GetLines())
            {
                if (!line.content.Contains(workspace.SearchText))
                    continue;

                var result = new SearchResult(
                    file.RelativePath,
                    line.lineNumber,
                    line.content);

                list.Add(result);
            }
        }
        return [..list];
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
        foreach (var action in agentActionCollection.actions)
        {
            found = true;
            var result = (string?)null;
            switch (action.type)
            {
                case "find":
                    result = await workspace.Search(action.searchText!);
                    break;

                case "find_and_replace":
                    result = await workspace.FindAndReplace(action.path!, action.searchText!, action.replaceText!);
                    break;

                case "find_and_replace_all":
                    result = await workspace.FindAndReplaceAll(action.searchText!, action.replaceText!);
                    break;

                case "open_file":
                    result = await workspace.OpenFile(action.path!);
                    break;

                case "create_or_update_file":
                    result = await workspace.CreateOrUpdateFile(action.path!, action.content!);
                    break;

                case "delete_file":
                    result = await workspace.DeleteFile(action.path!);
                    break;

                case "move_file":
                    result = await workspace.MoveFile(action.path!, action.newPath!);
                    break;

                //case "create_directory":
                //    result = await workspace.CreateDirectory(action.path!);
                //    break;

                //case "delete_directory":
                //    result = await workspace.RemoveDirectory(action.path!);
                //    break;

                case "partial_overwrite_file":
                    result = await workspace.PartialOverwriteFile(
                        action.path!,
                        action.startLine!.Value,
                        action.endLine!.Value,
                        action.content!
                    );
                    break;

                case "create_or_update_task":
                    result = await workspace.CreateOrUpdateTask(action.path!, action.content!);
                    break;

                case "delete_task":
                    result = await workspace.DeleteTask(action.path!);
                    break;

                default:
                    result = $"Action '{action.type}' not found";
                    found = false;
                    break;
            }
            if (action != null)
                list.Add(new AgentActionResult(action, result));
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