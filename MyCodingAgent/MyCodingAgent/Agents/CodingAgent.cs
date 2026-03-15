using MyCodingAgent.BaseAgents;
using MyCodingAgent.Compile;
using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.Ollama;
using System.Text.Json;

namespace MyCodingAgent.Agents;

public class CodingAgent(Workspace workspace) : JsonBaseAgent(workspace), IAgent
{
    public async Task<OllamaPrompt> GeneratePrompt(CompileResult compileResult)
    {
        var listAllFilesPrompt = await workspace.GetListAllFilesText();
        List<OllamaPromptMessage> messageList = 
        [
            // SYSTEM PROMPT
            new OllamaPromptMessage(
                nameof(OllamaAgentRole.system),
                $@"You are an autonomous software engineering agent operating inside a .NET 10 development workspace.

WORKFLOW

1. Understand the request
2. Inspect files if needed
3. Make minimal edits
4. Verify using search or open_file

Always prefer small incremental steps."),
            // USER ORIGINAL PROMPT
            new OllamaPromptMessage(nameof(OllamaAgentRole.user), workspace.UserPrompt),
            // DIRECTORY OVERVIEW
            new OllamaPromptMessage(nameof(OllamaAgentRole.user), $@"Current workspace files:
{listAllFilesPrompt}"),
        ];

        foreach (var responseResult in workspace.CodingHistory)
        {
            // AGENT RESPONSE 
            messageList.Add(new OllamaPromptMessage(
                nameof(OllamaAgentRole.assistant),
                $@"{responseResult.Response.message.content}{string.Join("\n", responseResult.Response.message.tool_calls.Select(b => $@"{{
  ""tool_call"": ""{b.function.name}"",
  ""args"": {JsonSerializer.Serialize(b.function.arguments, Program.JsonSerializeOptions)}
}}"))}"));

            //(TOOL) MCP RESPONSE
            if (responseResult.ToolResults.Length > 0)
            {
                messageList.Add(new OllamaPromptMessage(
                    nameof(OllamaAgentRole.tool),
                    $@"{string.Join("\n", responseResult.ToolResults.Select(b => $@"{{
  ""tool_call"": ""{b.toolCallFunction.name}"",
  ""result"": {JsonSerializer.Serialize(b.result.content, Program.JsonSerializeOptions)}
}}"))}"));
            }
            else
            {
                messageList.Add(new OllamaPromptMessage(
                    nameof(OllamaAgentRole.tool),
                    $@"Could not find any tool_call"));
            }
        }

        return new OllamaPrompt(
            [.. messageList],
            [.. tools.Select(a => a.ToDto())]);
    }

    public async Task<bool> ProcessResponse(OllamaPrompt prompt, OllamaResponse agentResponse)
    {
        var response = await GetAgentResponseResult(prompt, agentResponse);
        workspace.CodingHistory.Add(response);
        return response.ToolResults.Any(a => a.result.error == false);
    }
}