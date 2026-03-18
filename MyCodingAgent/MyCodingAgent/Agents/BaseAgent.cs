using MyCodingAgent.Helpers;
using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using System.Text.Json;

namespace MyCodingAgent.Agents;

public abstract class BaseAgent(Workspace Workspace, OllamaClient Client)
{
    protected abstract List<PromptResponseResults> History { get; }
    protected abstract IToolCall[] Tools { get; }

    protected Workspace Workspace { get; } = Workspace;
    protected OllamaClient Client { get; } = Client;

    protected static void AddHistoryAndToolCalls(List<OllamaMessage> messageList, List<PromptResponseResults> history, Tool[] tools, int maxTokens, int additionalSizeInBytes)
    {
        var notNullHistory = history
            .Where(a =>
                string.IsNullOrWhiteSpace(a.Response.message.content) == false ||
                (a.Response.message.tool_calls != null && a.Response.message.tool_calls.Any()))
            .ToList();

        var messagesJson = JsonSerializer.Serialize(messageList, DefaultJsonSerializerOptions.JsonSerializeOptionsIndented);
        var messagesJsonLength = messagesJson.Length;
        var toolsJson = OllamaClient.CreateToolsJson(tools);
        var toolsJsonLength = toolsJson.Length;
        var maxHistory = 0;
        int maxLongDesciptionPrompt = 0;
        var totalLength = messagesJsonLength + toolsJsonLength + additionalSizeInBytes;

        notNullHistory.Reverse(); // Lijst omdraaien zodat we vanaf het begin tellen
        var useShortContent = false;
        foreach (var responseResult in notNullHistory)
        {
            var responseJson = JsonSerializer.Serialize(CleanMessage(responseResult.Response.message), DefaultJsonSerializerOptions.JsonSerializeOptionsIndented);
            totalLength += responseJson.Length;

            // TOOL CALLS REPLIES
            if (responseResult.ToolCallResults.Any())
            {
                foreach (var toolCall in responseResult.ToolCallResults)
                {
                    var messageJson = JsonSerializer.Serialize(CreateToolCallbackMessage(useShortContent, toolCall), DefaultJsonSerializerOptions.JsonSerializeOptionsIndented);
                    totalLength += messageJson.Length;
                }
            }
            else
            {
                var messageJson = JsonSerializer.Serialize(CreateToolCallbackMessage(useShortContent, null), DefaultJsonSerializerOptions.JsonSerializeOptionsIndented);
                totalLength += messageJson.Length;
            }

            if (totalLength < maxTokens * 5 / 2)
                maxLongDesciptionPrompt++;
            else
            {
                useShortContent = true;
            }

            if (totalLength < maxTokens * 3)
                maxHistory++;
            else
            {
                break;
            }
        }

        notNullHistory.Reverse(); // Lijst weer normaal maken
        var i = notNullHistory.Count; // Dan terug tellen
        foreach (var responseResult in notNullHistory)
        {
            if (i > maxHistory)
            {
                i--;
                continue;
            }

            // AGENT RESPONSE 
            messageList.Add(CleanMessage(responseResult.Response.message));

            // TOOL CALLS REPLIES
            if (responseResult.ToolCallResults.Any())
            {
                foreach (var toolCall in responseResult.ToolCallResults)
                {
                    messageList.Add(CreateToolCallbackMessage(i > maxLongDesciptionPrompt, toolCall));
                }
            }
            else
            {
                messageList.Add(
                    CreateToolCallbackMessage(i > maxLongDesciptionPrompt, null));
            }

            i--;
        }

        messagesJson = JsonSerializer.Serialize(messageList, DefaultJsonSerializerOptions.JsonSerializeOptionsIndented);
        messagesJsonLength = messagesJson.Length;
        totalLength = messagesJsonLength + toolsJsonLength + additionalSizeInBytes;

    }

    private static OllamaMessage CleanMessage(OllamaMessage message)
    {
        var content = "Use tool_calls";
        if (message.tool_calls?.Any() == true)
        {
            content = string.Join(", ", message.tool_calls.Select(a => a.id));
        }
        if (!string.IsNullOrWhiteSpace(message.content))
        {
            content = message.content;
        }

        return new OllamaMessage(
            message.role,
            message.tool_call_id,
            content,
            null,
            message.tool_calls);
    }

    private static OllamaMessage CreateToolCallbackMessage(bool useShortContent, ToolCallResult? toolCall)
    {
        return new OllamaMessage(
            nameof(OllamaAgentRole.tool).ToLower(),
            toolCall?.tool_call.id,
            toolCall == null ? "No tool_calls found" : useShortContent ? toolCall.result.shortContent : toolCall.result.content,
            null,
            null);
    }

    protected async Task<PromptResponseResults> GetAgentResponseResult(OllamaPrompt prompt, OllamaResponse response, IToolCall[] tools)
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
