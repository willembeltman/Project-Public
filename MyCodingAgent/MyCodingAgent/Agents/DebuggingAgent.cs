using MyCodingAgent.BaseAgents;
using MyCodingAgent.Compile;
using MyCodingAgent.Helpers;
using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.Ollama;
using System.Text;
using System.Text.Json;

public class DebuggingAgent(Workspace workspace) : JsonBaseAgent(workspace), IAgent
{
    public async Task<OllamaPrompt> GeneratePrompt(CompileResult compileResult)
    {
        var listAllFilesPrompt = await workspace.GetListAllFilesText();
        var promptHelper = new PromptHelper(workspace);
        var errorView = new StringBuilder();
        await promptHelper.ShowErrorFiles(compileResult, errorView);
        List<OllamaPromptMessage> messageList =
        [
            // SYSTEM MESSAGE
            new OllamaPromptMessage(nameof(OllamaAgentRole.system), $@"You are a .NET 10 compiler repair agent."),

            // DIRECTORY OVERVIEW
            new OllamaPromptMessage(nameof(OllamaAgentRole.user), $@"Current workspace files:
{listAllFilesPrompt}")
        ];

        var i = workspace.DebugHistory.Count;

        foreach (var responseResult in workspace.DebugHistory)
        {
            // AGENT RESPONSE 
            messageList.Add(new OllamaPromptMessage(
                nameof(OllamaAgentRole.assistant),
                $@"{responseResult.Response.message.content}{string.Join("\n", responseResult.Response.message.tool_calls.Select(b => $@"{{
  ""tool_call"": ""{b.function.name}"",
  ""args"": {JsonSerializer.Serialize(b.function.arguments, Program.JsonSerializeOptionsNotIndented)}
}}"))}"));

            //(TOOL) MCP RESPONSE
            if (responseResult.ToolResults.Length > 0)
            {
                messageList.Add(new OllamaPromptMessage(
                    nameof(OllamaAgentRole.tool),
                    $@"{string.Join("\n", responseResult.ToolResults.Select(b => $@"{{
  ""tool_call"": ""{b.toolCallFunction.name}"",
  ""args"": {JsonSerializer.Serialize(b.toolCallFunction.arguments, Program.JsonSerializeOptionsNotIndented)}
}}
{(i > 1 ? b.result.shortContent : b.result.content)}"))}"));
            }
            else
            {
                messageList.Add(new OllamaPromptMessage(
                    nameof(OllamaAgentRole.tool),
                    $@"Could not find any tool_call"));
            }

            i--;
        }

        // ERROR OVERVIEW
        messageList.Add(
            new OllamaPromptMessage(nameof(OllamaAgentRole.user), $@"{errorView}GOAL
Make the code compile successfully.
Do not change behavior unless required."));

        return new OllamaPrompt(
            [.. messageList],
            [.. tools.Select(a => a.ToDto())]);
    }

    public async Task<bool> ProcessResponse(OllamaPrompt prompt, OllamaResponse agentResponse)
    {
        var response = await GetAgentResponseResult(prompt, agentResponse);
        workspace.DebugHistory.Add(response);
        return response.ToolResults.Any(a => a.result.error == false);
    }
}