using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;

namespace MyCodingAgent.ToolCalls;

public class WorkIsDone_Tool(Workspace workspace) : IToolCall
{
    public string Name
        => "work_is_done";

    public string Description
        => "The definitive signal that all planned sub-tasks have been successfully executed and verified. Use this tool to submit your final results to the user once you are confident that every requirement in the prompt has been met and the workspace is in a stable state.";
    public ToolParameter[] Parameters { get; } = [];
    public async Task<ToolResult> Invoke(OllamaToolCall toolCall)
    {
        var toolArguments = toolCall.function.arguments;
        workspace.WorkIsDone = true;
        return new ToolResult("OK DONE!", "OK DONE!", false);
    }
}
