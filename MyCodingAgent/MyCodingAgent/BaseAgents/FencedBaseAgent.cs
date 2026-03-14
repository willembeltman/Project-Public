using MyCodingAgent.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace MyCodingAgent.BaseAgents;

public class FencedBaseAgent(
    Workspace workspace)
{
    protected JsonSerializerOptions JsonDeserializerOptions { get; } =
        new() { PropertyNameCaseInsensitive = true };

    protected Workspace workspace { get; } = workspace;

    protected string GetActionsText()
    {
        return @"
You interact with this system through a tool-DSL with fenced blocks command protocol.

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

EXAMPLE RESPONSE

# action
```find
searchText: A search is always one line
```

# action
```find_and_replace
path: Program.cs
searchText: Using multiple lines for replace doesn't work
replaceText: Using multiple lines for replace is too fragile
```

# action
```partial_overwrite_file
path: Program.cs
startLine: 8
endLine: 9
content:
    Console.WriteLine(""This is 1 line of code with 4 leading spaces"");
```

IMPORTANT RULES
1. Always lead with # action
2. Do NOT include explanations outside the actions.
3. Only use the actions listed above.
4. Never assume file contents, open the file first.
5. Target .NET 10.
6. If you need to modify a file you MUST first open_file.
7. You may return multiple actions in a single response.
8. Only use the parameters defined for each action.
9. Every action MUST be inside a fenced block.
";
    }

    public async Task<bool> ProcessResponse(AgentResponse agentResponse)
    {
        (var agentActionCollection, var parseError) =
            TryParseActions(agentResponse.responseText);

        if (agentActionCollection == null || parseError != null)
        {
            workspace.AgentResponseResults.Add(
                new AgentResponseResult(agentResponse, parseError, []));
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
                    result = await workspace.FindAndReplace(
                        action.path!,
                        action.searchText!,
                        action.replaceText!);
                    break;

                case "find_and_replace_all":
                    result = await workspace.FindAndReplaceAll(
                        action.searchText!,
                        action.replaceText!);
                    break;

                case "open_file":
                    result = await workspace.OpenFile(action.path!);
                    break;

                case "create_or_update_file":
                    result = await workspace.CreateOrUpdateFile(
                        action.path!,
                        action.content!);
                    break;

                case "delete_file":
                    result = await workspace.DeleteFile(action.path!);
                    break;

                case "move_file":
                    result = await workspace.MoveFile(
                        action.path!,
                        action.newPath!);
                    break;

                case "partial_overwrite_file":
                    result = await workspace.PartialOverwriteFile(
                        action.path!,
                        action.startLine!.Value,
                        action.endLine!.Value,
                        action.content!);
                    break;

                case "create_or_update_task":
                    result = await workspace.CreateOrUpdateTask(
                        action.path!,
                        action.content!);
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

        workspace.AgentResponseResults.Add(
            new AgentResponseResult(agentResponse, null, [.. list]));

        return found;
    }
    private (AgentActionCollection? response, string? parseError) TryParseActions(string input)
    {
        try
        {
            var actions = new List<AgentAction>();

            var matches = Regex.Matches(
                input,
                @"```(\w+)\s*(.*?)```",
                RegexOptions.Singleline);

            foreach (Match match in matches)
            {
                var type = match.Groups[1].Value.Trim();
                var body = match.Groups[2].Value;

                var dict = ParseKeyValue(body);

                actions.Add(new AgentAction
                {
                    type = type,
                    path = dict.GetValueOrDefault("path"),
                    searchText = dict.GetValueOrDefault("searchText"),
                    replaceText = dict.GetValueOrDefault("replaceText"),
                    content = dict.GetValueOrDefault("content"),
                    newPath = dict.GetValueOrDefault("newPath"),
                    startLine = TryInt(dict.GetValueOrDefault("startLine")),
                    endLine = TryInt(dict.GetValueOrDefault("endLine"))
                });
            }

            return new(new AgentActionCollection { actions = actions }, null);
        }
        catch (Exception ex)
        {
            return new(null, ex.Message);
        }
    }

    private int? TryInt(string? v)
    {
        if (int.TryParse(v, out var result))
        {
            return result;
        }
        return null;
    }

    private Dictionary<string, string> ParseKeyValue(string input)
    {
        var dict = new Dictionary<string, string>();

        using var reader = new StringReader(input);

        string? line;
        string? currentKey = null;
        var sb = new StringBuilder();

        while ((line = reader.ReadLine()) != null)
        {
            if (line.Contains(":") && !line.StartsWith(" "))
            {
                if (currentKey != null)
                    dict[currentKey] = sb.ToString().Trim();

                var parts = line.Split(':', 2);
                currentKey = parts[0].Trim();
                sb.Clear();

                if (parts.Length > 1)
                    sb.AppendLine(parts[1]);
            }
            else
            {
                sb.AppendLine(line);
            }
        }

        if (currentKey != null)
            dict[currentKey] = sb.ToString().Trim();

        return dict;
    }
}