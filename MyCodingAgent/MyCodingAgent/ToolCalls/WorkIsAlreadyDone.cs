using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;

namespace MyCodingAgent.ToolCalls;

public class WorkIsAlreadyDone(Workspace workspace) : ITool
{
    public string Name
        => "work_is_already_done";
    public string Description
        => "Signals the final completion of the entire project. Use this tool only when every sub-task is finished, the code is verified (compiled/tested), and all requirements from the user's initial prompt have been fully met.";
    public ToolParameter[] Parameters { get; } = [];

    public async Task<ToolResult> Invoke(OllamaToolCall toolCall)
    {
        var toolArguments = toolCall.function.arguments;
        workspace.PlanningIsDone = true;
        workspace.WorkIsDone = true;
        await workspace.Save();
        return new ToolResult("OK DONE!", "OK DONE!", false);
    }
}
