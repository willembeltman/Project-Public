using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;

namespace MyCodingAgent.ToolCalls;

public class DebugAgentIsDone_Tool(Workspace workspace) : IToolCall
{
    public string Name
        => "debugger_is_done";
    public string Description
        => "The definitive signal that all bugs have been successfully executed and verified. Use this tool to submit your final results to the coding agent.";
    public ToolParameter[] Parameters { get; } = 
    [
        new ("content", "string", "Review of your fixes", null, true)
    ];
    public async Task<ToolResult> Invoke(OllamaToolCall toolCall)
    {
        var toolArguments = toolCall.function.arguments;
        if (toolArguments.content == null)
            return new ToolResult(
                "Error parameter content is not supplied.",
                "Error parameter content is not supplied.",
                true);

        var history = workspace.CodingHistory.LastOrDefault();
        if (history != null)
        {
            history.ToolCallResults.Add(
                new ToolCallResult(toolCall,
                    new ToolResult(
                        $"Your changes resulted in a error, so the debugger agent has fixed them.\r\nThis is his rapport about the fix:\r\n{toolArguments.content}",
                        $"Your changes resulted in a error, so the debugger agent has fixed them.",
                        false)));
        }
        workspace.Debugging = false;

        return new ToolResult("OK DONE!", "OK DONE!", false);
    }
}
