using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;

namespace MyCodingAgent.ToolCalls;

public class CompileWorkspace(Workspace workspace) : ITool
{
    public string Name 
        => "compile_workspace";
    public string Description
        => "Builds the project using .sln or .csproj in workspace root. Use this to catch errors after making code changes.";
    public ToolParameter[] Parameters { get; } = [];

    public async Task<ToolResult> Invoke(OllamaToolCallFunctionArguments toolArguments)
    {
        var compileResult = await workspace.Compile();
        return new ToolResult(
            compileResult.Output,
            compileResult.Errors.Count > 0 ? "Compiled with errors" : "Compiled succesfully",
            compileResult.Errors.Count > 0);
    }
}
