using MyCodingAgent.BaseAgents;
using MyCodingAgent.Compile;
using MyCodingAgent.Models;

namespace MyCodingAgent.Agents;

public class IsFinishedAgent(
    Workspace workspace) 
    : YesNoAgent
{
    public async Task<string> GeneratePrompt(CompileResult compileResult)
    {
        return $@"The current source contents is:{(workspace.Files.Count == 0
            ? @$"
<No files in directory>"
            : string.Join(Environment.NewLine, workspace.Files.Select(a => $@"

File '{a.RelativePath}':
{a.GetFileContent}")))}{(string.IsNullOrWhiteSpace(compileResult.Output) ? "" : $@"

The current compile result is:
{compileResult}")}

The user prompt is:
{workspace.UserPrompt}

Is the prompt satisfied? Reply [YES] or [NO]

Response must include the [ and ] signs
";
    }
}