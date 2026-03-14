using MyCodingAgent.Agents;
using MyCodingAgent.Compile;
using MyCodingAgent.Models;
using MyCodingAgent.Helpers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

public class DebuggingAgent(Workspace workspace) : IModifyAgent
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

        foreach (var fileGroup in compileResult.Errors.GroupBy(a => a.File))
        {
            var relativePath = fileGroup.Key;
            if (string.IsNullOrWhiteSpace(relativePath)) continue;

            var file = workspace.GetFile(relativePath);
            if (file != null)
            {
                sb.AppendLine(relativePath);
                sb.AppendLine("ERRORS");
                foreach (var error in fileGroup)
                {
                    sb.AppendLine(error.FullError.TrimEnd('\n').TrimEnd('\r'));
                }

                sb.AppendLine();
                sb.AppendLine("CODE");
                var fileContent = await file.GetFileContent();
                foreach (var line in fileContent.GetLines())
                {
                    sb.AppendLine($"{line.lineNumber,3}|{line.content}");
                }
                sb.AppendLine();
            }
        }

        var workspaceText = sb.ToString();

        return $@"You are a .NET 10 compiler repair agent.

GOAL
Make the code compile successfully.
Do not change behavior unless required.

IMPORTANT RULES
1. Your response MUST be valid JSON
2. The JSON MUST contain an array called ""actions""
3. Only use the actions listed below
4. Prefer minimal edits
5. If modifying a file prefer partial_overwrite_file

AVAILABLE ACTIONS
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
Update Program.cs

Response:
{{
  ""actions"": [
    {{
      ""type"": ""partial_overwrite_file"",
      ""path"": ""Program.cs"",
      ""startLine"": 7,
      ""endLine"": 8,
      ""content"": ""internal class Program""
    }}
  ]
}}

FILES
{workspaceText}Respond ONLY with JSON.

The first character of your response must be ""{{""
The last character must be ""}}""
Do not end response with ```";
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
        foreach (var agentAction in agentActionCollection.actions)
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