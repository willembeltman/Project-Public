using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.Ollama;
using MyCodingAgent.Tools;
using System.Text.Json;

namespace MyCodingAgent.Agents;

public class Agent(
    Workspace workspace)
{
    protected Workspace workspace { get; } = workspace;
   
    protected static void AddHistory(List<OllamaPromptMessage> messageList, List<AgentResponseResult> history)
    {
        var i = history.Count;
        foreach (var responseResult in history)
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
    }


    protected async Task<AgentResponseResult> GetAgentResponseResult(OllamaPrompt prompt, OllamaResponse response, ITool[] tools)
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
