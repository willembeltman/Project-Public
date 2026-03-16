using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.Ollama;

namespace MyCodingAgent.ToolCalls;

public class WorkIsDone(Workspace workspace) : ITool
{
    public string Name
        => "work_is_done";
    public string Desciption
        => "indicate all work is done, all user prompts are satisfied";
    public ToolParameter[] Parameters { get; } = [];
    public async Task<ToolResult> Invoke(OllamaToolCallFunctionArguments toolArguments)
    {
        workspace.UserPromptDone = true;
        await workspace.Save();
        return new ToolResult("OK DONE!", "OK DONE!", false);
    }
}
