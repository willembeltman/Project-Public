using MyCodingAgent.Models;
using System.Text.Json;

namespace MyCodingAgent.BaseAgents;

public class JsonBaseAgent(
    Workspace workspace)
{
    protected JsonSerializerOptions JsonDeserializerOptions { get; } = new() { PropertyNameCaseInsensitive = true };
    protected Workspace workspace { get; } = workspace;

    protected string GetActionsText()
    {
        return @"
You interact with this system through a JSON command protocol.

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
ask_developer_extra_information(content: string)

RESPONSE FORMAT
{
  ""actions"": []
}

EXAMPLE RESPONSE
User request:
Open Program.cs

Response:
{
  ""actions"": [
    {
      ""type"": ""open_file"",
      ""path"": ""Program.cs""
    }
  ]
}

IMPORTANT RULES
1. Your response MUST be valid JSON.
2. The JSON MUST contain an array called ""actions"".
3. Do NOT include explanations outside the JSON.
4. Only use the actions listed above.
5. Never assume file contents, open the file first.
6. Target .NET 10
7. Do not find_and_replace large textblocks
8. If you need to modify a file you MUST first open_file.";
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
                    result = await workspace.Find(action.searchText!);
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

                case "ask_developer_extra_information":
                    result = await workspace.AskDeveloper(action.content!);
                    break;

                default:
                    result = $"Action '{action.type}' not found";
                    found = false;
                    break;
            }
            if (result != null)
                list.Add(new AgentActionResult(action, result));
        }
        workspace.AgentResponseResults.Add(new AgentResponseResult(agentResponse, null, [.. list]));
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

        int start = input.IndexOf('{');
        int end = input.LastIndexOf('}');

        if (start >= 0 && end > start)
            input = input.Substring(start, end - start + 1);

        return input;
    }
}
