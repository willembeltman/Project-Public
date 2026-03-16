using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;

namespace MyCodingAgent.ToolCalls;

public class ListAllFiles(Workspace workspace) : ITool
{
    public string Name
        => "list_all_files";
    public string Description
        => "Returns a flat list of all file paths in the workspace. Use this to discover the project structure.";
    public ToolParameter[] Parameters { get; } = [];
    public async Task<ToolResult> Invoke(OllamaToolCallFunctionArguments toolArguments)
    {
        var listAllFilesText = await workspace.GetListAllFilesText();
        return new ToolResult(listAllFilesText, "Shown all files", false);
    }
}
