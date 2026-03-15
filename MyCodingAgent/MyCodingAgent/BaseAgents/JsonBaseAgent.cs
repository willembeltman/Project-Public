using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.Ollama;
using MyCodingAgent.Tools;

namespace MyCodingAgent.BaseAgents;

public class JsonBaseAgent(
    Workspace workspace)
{
    protected Workspace workspace { get; } = workspace;
    protected ITool[] tools { get; } =
    [
        new ListAllFiles(workspace),
        new Find(workspace),
        new FindAndReplace(workspace),
        new FindAndReplaceAll(workspace),
        new OpenFile(workspace),
        new CreateOrUpdateFile(workspace),
        new PartialOverwriteFile(workspace),
        new MoveFile(workspace),
        new DeleteFile(workspace),
        new CompileWorkspace(workspace),
        new AskDeveloperForExtraInformation()
    ];

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

    protected async Task<AgentResponseResult> GetAgentResponseResult(OllamaPrompt prompt, OllamaResponse response)
    {
        var list = new List<AgentResponseToolResult>();
        foreach (var tool_call in response.message.tool_calls)
        {
            var toolName = tool_call.function.name;
            var toolArguments = tool_call.function.arguments;

            var tool = tools.FirstOrDefault(a => a.Name == toolName);
            if (tool == null)
            {
                list.Add(new AgentResponseToolResult(
                    tool_call.function,
                    new ToolResult(
                        $"Could not find tool '{toolName}'",
                        $"Could not find tool",
                        true)));
                continue;
            }
            else
            {
                var toolResult = await tool.Invoke(toolArguments);
                list.Add(new AgentResponseToolResult(
                    tool_call.function,
                    toolResult));
            }
        }

        return new AgentResponseResult(
            prompt,
            response,
            [.. list]);
    }
}
