using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;

namespace MyCodingAgent.ToolCalls;

public class CompileWorkspace(Workspace workspace) : ITool
{
    public string Name 
        => "compile_workspace";
    public string Desciption
        => "search the root of your workspace for a .sln or .csproj file and tries to compile it, then return the compilation results";
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
