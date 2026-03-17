using MyCodingAgent.Helpers;
using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using System.Text.Json;

namespace MyCodingAgent.Agents;

public abstract class BaseAgent(Workspace Workspace)
{
    protected abstract List<PromptResponseResults> History { get; }
    protected abstract ITool[] Tools { get; }

    protected Workspace Workspace { get; } = Workspace;

    protected static void AddHistoryAndToolCalls(List<OllamaMessage> messageList, List<PromptResponseResults> history, Tool[] tools, int maxTokens, int additionalSizeInBytes)
    {
        var messagesJson = JsonSerializer.Serialize(messageList, DefaultJsonSerializerOptions.JsonSerializeOptionsIndented);
        var messagesJsonLength = messagesJson.Length;
        var toolsJson = OllamaClient.GetToolsJson(tools);
        var toolsJsonLength = toolsJson.Length;
        var maxHistory = 0;
        int maxLongDesciptionPrompt = 0;
        var useShortContent = false;
        var length = messagesJsonLength + toolsJsonLength + additionalSizeInBytes;

        history.Reverse();
        foreach (var responseResult in history)
        {
            var responseJson = JsonSerializer.Serialize(responseResult.Response.message, DefaultJsonSerializerOptions.JsonSerializeOptionsIndented);
            length += responseJson.Length;

            // TOOL CALLS REPLIES
            foreach (var toolCall in responseResult.ToolCallResults)
            {
                var message = new OllamaMessage(
                    nameof(OllamaAgentRole.tool).ToLower(),
                    toolCall.tool_call.id,
                    useShortContent ? toolCall.result.shortContent : toolCall.result.content,
                    null,
                    null);
                var messageJson = JsonSerializer.Serialize(message, DefaultJsonSerializerOptions.JsonSerializeOptionsIndented);

                length += messageJson.Length;
            }

            if (length < maxTokens * 3)
                maxLongDesciptionPrompt++;
            else
            {
                useShortContent = true;
            }

            if (length < maxTokens * 4)
                maxHistory++;
            else
            {

            }
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
                    var toolResult = await tool.Invoke(tool_call);
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
