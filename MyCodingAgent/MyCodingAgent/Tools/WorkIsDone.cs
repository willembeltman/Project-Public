using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.Ollama;

namespace MyCodingAgent.Tools;

public class WorkIsDone(Workspace workspace) : ITool
{
    public string Name
        => "work_is_done";
    public string Desciption
        => "signals all work is done and needs user validation";
    public ToolParameter[] Parameters { get; } = [];
    public async Task<ToolResult> Invoke(OllamaResponseMessageToolCallFunctionArguments toolArguments)
    {
        workspace.UserPromptDone = true;
        await workspace.Save();
        return new ToolResult("OK DONE!", "OK DONE!", false);
    }
}
