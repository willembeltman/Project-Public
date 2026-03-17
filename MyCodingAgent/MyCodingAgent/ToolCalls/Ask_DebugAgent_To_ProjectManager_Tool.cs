using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;

namespace MyCodingAgent.ToolCalls;

public class Ask_DebugAgent_To_ProjectManager_Tool(Workspace workspace) : IToolCall
{
    public string Name
        => "ask_project_manager";

    public string Description
        => "Requests clarification or missing technical details from the Project Manager. Use this when a sub-task is ambiguous, a dependency is missing, or you encounter an architectural decision you cannot make alone. This pauses your execution until feedback is provided.";

    public ToolParameter[] Parameters { get; } =
    [
        new ("content", "string", "The specific question or missing information needed to proceed with the task.")
    ];

    public async Task<ToolResult> Invoke(OllamaToolCall toolCall)
    {
        var toolArguments = toolCall.function.arguments;
        if (toolArguments.content == null)
            return new ToolResult(
                "parameter content is not supplied.",
                "parameter content is not supplied.",
                true);

        if (toolCall.id == null)
            throw new Exception("eeeuhm...");

        workspace.DebugAgent_WaitingFor_ProjectManagerAgent_Answer =
            new(toolCall.id, toolArguments.content);

        var answer = "Waiting for answer...";
        return new ToolResult(
            answer,
            answer,
            false);
    }
}