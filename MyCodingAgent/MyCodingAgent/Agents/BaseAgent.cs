using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.Ollama;
using System.Text.Json;

namespace MyCodingAgent.Agents;

public abstract class BaseAgent(Workspace workspace)
{
    protected abstract List<PromptResponseResults> history { get; }
    protected abstract ITool[] tools { get; }

    protected Workspace workspace { get; } = workspace;

    protected static void AddHistoryAndToolCalls(List<OllamaMessage> messageList, List<PromptResponseResults> history, Tool[] tools, int maxTokens, int additionalSizeInBytes)
    {
        var messagesJson = JsonSerializer.Serialize(messageList, Program.JsonSerializeOptions);
        var messagesJsonLength = messagesJson.Length;
        var toolsJson = OllamaService.GetToolsJson(tools);
        var toolsJsonLength = toolsJson.Length;
        var maxHistory = 0;
        int maxLongDesciptionPrompt = 0;

        history.Reverse();
        foreach (var responseResult in history)
        {
            var messageJson = JsonSerializer.Serialize(responseResult.Response.message, Program.JsonSerializeOptions);
            var length = 
                messagesJsonLength + toolsJsonLength + additionalSizeInBytes +
                messageJson.Length;

            // TOOL CALLS REPLIES
            foreach (var toolCall in responseResult.ToolCallResults)
            {
                var message = new OllamaMessage(
                    nameof(OllamaAgentRole.tool).ToLower(),
                    toolCall.tool_call.id,
                    maxHistory > maxLongDesciptionPrompt ? toolCall.result.shortContent : toolCall.result.content,
                    null,
                    null);

                length += JsonSerializer.Serialize(message, Program.JsonSerializeOptions).Length;
            }

            if (length < maxTokens * 3)
                maxLongDesciptionPrompt++;

            if (length < maxTokens * 4)
                maxHistory++;
            else
                break;
        }

        history.Reverse();
        var i = history.Count;
        foreach (var responseResult in history)
        {
            if (i > maxHistory)
            {
                i--;
                continue;
            }

            // AGENT RESPONSE 
            messageList.Add(responseResult.Response.message);

            // TOOL CALLS REPLIES
            foreach (var toolCall in responseResult.ToolCallResults)
            {
                messageList.Add(new OllamaMessage(
                    nameof(OllamaAgentRole.tool).ToLower(),
                    toolCall.tool_call.id,
                    i > maxLongDesciptionPrompt ? toolCall.result.shortContent : toolCall.result.content,
                    null,
                    null));
            }

            i--;
        }
    }

    protected async Task<PromptResponseResults> GetAgentResponseResult(OllamaPrompt prompt, OllamaResponse response, ITool[] tools)
    {
        var list = new List<ToolCallResult>();
        if (response.message.tool_calls != null)
        {
            foreach (var tool_call in response.message.tool_calls)
            {
                var toolName = tool_call.function.name;
                var toolArguments = tool_call.function.arguments;

                var tool = tools.FirstOrDefault(a => a.Name == toolName);
                if (tool == null)
                {
                    list.Add(new ToolCallResult(
                        tool_call,
                        new ToolResult(
                            $"Could not find tool '{toolName}'",
                            $"Could not find tool",
                            true)));
                    continue;
                }
                else
                {
                    var toolResult = await tool.Invoke(toolArguments);
                    list.Add(new ToolCallResult(
                        tool_call,
                        toolResult));
                }
            }
        }
        return new PromptResponseResults(
            prompt,
            response,
            [.. list]);
    }
}
