using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.Ollama;

namespace MyCodingAgent.Agents;

public class _BaseAgent(
    Workspace workspace)
{
    protected Workspace workspace { get; } = workspace;

    protected static void AddHistory(List<OllamaMessage> messageList, List<AgentResponseResult> history, int maxLongDesciptionPrompt, int maxLongDescriptionResponse, int maxHistory)
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
            messageList.Add(responseResult.response.message);

            // TOOL CALLS REPLIES
            foreach (var toolCall in responseResult.ToolResults)
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
                    list.Add(new AgentResponseToolResult(
                        tool_call,
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
