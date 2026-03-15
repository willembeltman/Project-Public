using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.Ollama;

namespace MyCodingAgent.Tools;

public class ListAllFiles(Workspace workspace) : ITool
{
    public string Name
        => "list_all_files";
    public string Desciption
        => "retrieves a list of all files inside the workspace";
    public ToolParameter[] Parameters { get; } = [];
    public async Task<ToolResult> Invoke(OllamaResponseMessageToolCallFunctionArguments toolArguments)
    {
        var listAllFilesText = await workspace.GetListAllFilesText();
        return new ToolResult(listAllFilesText, "Shown all files", false);
    }
}
