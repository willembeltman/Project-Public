using MyCodingAgent.Compile;
using MyCodingAgent.Models;

namespace MyCodingAgent.Agents;

public class IsFinishedAgent(
    Workspace workspace) 
{

    public bool IsDone { get; set; }

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

    public async Task<bool> ProcessResponse(AgentResponse response)
    {
        if (response.responseText.Contains("[NO]", StringComparison.InvariantCultureIgnoreCase))
        {
            IsDone = false;
            return true;
        }
        else if (response.responseText.Contains("[YES]", StringComparison.InvariantCultureIgnoreCase))
        {
            IsDone = true;
            return true;
        }
        return false;
    }
}