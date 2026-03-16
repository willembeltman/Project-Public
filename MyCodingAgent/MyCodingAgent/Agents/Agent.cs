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
   
    protected static void AddHistory(List<OllamaPromptMessage> messageList, List<AgentResponseResult> history, int maxLongDesciptionPrompt, int maxLongDescriptionResponse, int maxHistory)
    {
        var i = history.Count;
        foreach (var responseResult in history)
        {
            if (i > maxHistory)
            {
                i--;
                continue;
            }

            // AGENT RESPONSE 
            var toolCall =
                responseResult.Response.message.tool_calls == null 
                ? string.Empty 
                : string.Join("\n", responseResult.Response.message.tool_calls.Select(b => $@"{{
  ""tool_call"": ""{b.function.name}""{(i > maxLongDescriptionResponse ? "" : $@",
  ""args"": {JsonSerializer.Serialize(b.function.arguments, Program.JsonSerializeOptionsNotIndented)}")}
}}"));

            messageList.Add(new OllamaPromptMessage(
                nameof(OllamaAgentRole.assistant),
                $@"{responseResult.Response.message.content}{toolCall}"));

            //(TOOL) MCP RESPONSE
            if (responseResult.ToolResults.Length > 0)
            {
                messageList.Add(new OllamaPromptMessage(
                    nameof(OllamaAgentRole.tool),
                    $@"{string.Join("\n", responseResult.ToolResults.Select(b => $@"{{
  ""tool_call"": ""{b.function.name}""{(i > maxLongDescriptionResponse ? "" : $@",
  ""args"": {JsonSerializer.Serialize(b.function.arguments, Program.JsonSerializeOptionsNotIndented)}")}
}}
{(i > maxLongDesciptionPrompt ? b.result.shortContent : b.result.content)}"))}"));
            }
            else
            {
                if (string.IsNullOrEmpty(responseResult.Response.message.content))
                    messageList.Add(new OllamaPromptMessage(
                        nameof(OllamaAgentRole.tool),
                        $@"Could not find any tool_call"));
                else
                    messageList.Add(new OllamaPromptMessage(
                        nameof(OllamaAgentRole.tool),
                        $@"Could not find any tool_call, you should not reply normally, use the tool_calls"));
            }

            i--;
        }
    }


    protected async Task<AgentResponseResult> GetAgentResponseResult(OllamaPrompt prompt, OllamaResponse response, ITool[] tools)
    {
        var list = new List<AgentResponseToolResult>();
        if (response.message.tool_calls != null)
        {
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
        }
        return new AgentResponseResult(
            prompt,
            response,
            [.. list]);
    }
}
