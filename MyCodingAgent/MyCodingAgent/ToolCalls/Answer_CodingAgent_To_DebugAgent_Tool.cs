using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;

namespace MyCodingAgent.ToolCalls;

public class Answer_CodingAgent_To_DebugAgent_Tool(Workspace workspace) : IToolCall
{
    public string Name
        => "answer_debugger_question";

    public string Description
        => "Provides the official response or missing technical details to a Debug Agent's request. Use this to clarify requirements, provide architectural guidance, or resolve blockers that prevent the coder from proceeding.";

    public ToolParameter[] Parameters { get; } =
    [
        new ("content", "string", "The detailed answer or instruction that will be sent back to the coding agent.")
    ];

    public async Task<ToolResult> Invoke(OllamaToolCall toolCall)
    {
        var toolArguments = toolCall.function.arguments;
        if (toolArguments.content == null)
            return new ToolResult(
                "parameter content is not supplied.",
                "parameter content is not supplied.",
                true);

        if (workspace.DebugAgent_WaitingFor_CoderAgent_Answer == null)
            throw new Exception("Oh ooh...");

        var debuggerToolCall = workspace.DebugHistory
            .SelectMany(a => a.ToolCallResults)
            .FirstOrDefault(a => a.tool_call.id == workspace.DebugAgent_WaitingFor_CoderAgent_Answer.ToolCallId);
        if (debuggerToolCall == null)
            throw new Exception("Oh ooh...");

        debuggerToolCall.result.content = toolArguments.content;
        workspace.DebugAgent_WaitingFor_CoderAgent_Answer = null;

        return new ToolResult(
            $"Updated subtask '{toolArguments.id}'",
            $"Updated subtask",
            false);
    }
}