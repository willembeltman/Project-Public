using MyCodingAgent.Agents;
using MyCodingAgent.Models;
using MyCodingAgent.Ollama;
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
{
  ""actions"": [
    {
      ""type"": ""find"",
      ""searchText"": ""MyClass""
    },
    {
      ""type"": ""find_and_replace"",
      ""path"": ""Program.cs"",
      ""searchText"": ""Hello World"",
      ""replaceText"": ""Hello Everybody""
    },
    {
      ""type"": ""partial_overwrite_file"",
      ""path"": ""Program.cs""
      ""startLine"": 8,
      ""endLine"": 9,
      ""content"": ""\r\n    Console.WriteLine(""Please use small incremental updates"");\r\n""
    },
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
8. If you need to modify a file you MUST first open_file.
";
    }
    protected string GetReducedActionsText()
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
ask_developer_extra_information(content: string)

RESPONSE FORMAT
{
  ""actions"": []
}

EXAMPLE RESPONSE
{
  ""actions"": [
    {
      ""type"": ""find"",
      ""searchText"": ""MyClass""
    },
    {
      ""type"": ""find_and_replace"",
      ""path"": ""Program.cs"",
      ""searchText"": ""Hello World"",
      ""replaceText"": ""Hello Everybody""
    },
    {
      ""type"": ""partial_overwrite_file"",
      ""path"": ""Program.cs""
      ""startLine"": 8,
      ""endLine"": 9,
      ""content"": ""\r\n    Console.WriteLine(""Please use small incremental updates"");\r\n""
    },
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
8. If you need to modify a file you MUST first open_file.
";
    }

    protected string CreateToolsString()
    {
        return $@"[{string.Join(",", GetToolDefinitions().Select(tool => $@"
    {{
        ""type"": ""function"",
        ""function"": {{
            ""name"": ""{tool.Name}"",
            ""description"": ""{tool.Desciption}"",
            ""parameters"": {{
                ""type"": ""object"",
                ""properties"": {{{string.Join(",", tool.Parameters.Select(parameter => $@"
                    ""{parameter.Name}"": {{
                        ""type"": ""{parameter.Type}"",
                        ""description"": ""{parameter.Description}""
                    }}"))}
                }},
                ""required"": [{string.Join(",", tool.Parameters.Select(parameter => $@"""{parameter.Name}"""))}]
            }}
        }}
    }}"))}
]";
    }
    protected Tool[] GetToolDefinitions() =>
    [
        new ("find", "search for a specific string inside all files, result will be in next message",
            [
                new ("searchText", "string", "the specific string")
            ]),

            new ("find_and_replace", "searches for a specific string inside given file and replaces it with the replacement string",
            [
                new ("path", "string", "path to the file to search in"),
                new ("searchText", "string", "the search string"),
                new ("replaceText", "string", "the replacement string")
            ]),

            new ("find_and_replace_all", "searches for a specific string inside all files and replaces it with the replacement string",
            [
                new ("searchText", "string", "the search string"),
                new ("replaceText", "string", "the replacement string")
            ]),

            new ("open_file", "opens a file and returns its content",
            [
                new ("path", "string", "path to the file to open")
            ]),

            new ("create_or_update_file", "creates a new file or overwrites an existing file with the provided content",
            [
                new ("path", "string", "path to the file"),
                new ("content", "string", "full content of the file")
            ]),

            new ("partial_overwrite_file", "overwrites a specific line range inside a file",
            [
                new ("path", "string", "path to the file"),
                new ("startLine", "number", "first line to overwrite (inclusive)"),
                new ("endLine", "number", "last line to overwrite (inclusive)"),
                new ("content", "string", "replacement content for the specified line range")
            ]),

            new ("move_file", "moves or renames a file",
            [
                new ("path", "string", "current path of the file"),
                new ("newPath", "string", "new path of the file")
            ]),

            new ("delete_file", "deletes a file",
            [
                new ("path", "string", "path to the file")
            ]),

            new ("create_or_update_task", "creates or updates a task file used to track progress",
            [
                new ("path", "string", "path of the task file"),
                new ("content", "string", "content of the task file")
            ]),

            new ("delete_task", "deletes a task file",
            [
                new ("path", "string", "path of the task file")
            ]),

            new ("ask_developer_extra_information", "asks the developer for additional information when the task cannot continue",
            [
                new ("content", "string", "question or information request for the developer")
            ])
    ];

    public async Task<bool> ProcessResponse(OllamaPrompt prompt, AgentResponse agentResponse)
    {
        //(var agentActionCollection, var parseError) = TryParseActions(agentResponse.responseText);

        //if (agentActionCollection == null || parseError != null)
        //{
        //    workspace.AgentResponseResults.Add(
        //        new AgentResponseResult(
        //            prompt.messages.Last(a => a.role == OllamaAgentRole.user).content,
        //            agentResponse, 
        //            parseError, 
        //            []));
        //    return false;
        //}

        var found = false;
        var list = new List<AgentActionResult>();
        foreach (var tool_call in agentResponse.message.tool_calls)
        {
            var actionType = tool_call.function.name;
            var action = tool_call.function.arguments;

            found = true;
            var result = (string?)null;
            switch (actionType)
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
                    result = $"Action '{actionType}' not found";
                    found = false;
                    break;
            }
            if (result != null)
                list.Add(new AgentActionResult(tool_call.function, result));
        }
        workspace.AgentResponseResults.Add(
            new AgentResponseResult(
                    prompt.messages.Last(a => a.role == nameof(OllamaAgentRole.user)).content,
                    agentResponse,
                    null, 
                    [.. list]));
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
