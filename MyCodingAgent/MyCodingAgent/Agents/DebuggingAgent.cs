using MyCodingAgent.Compile;
using MyCodingAgent.Models;
using MyCodingAgent.Helpers;
using System.Text;
using MyCodingAgent.Interfaces;
using MyCodingAgent.BaseAgents;

public class DebuggingAgent(Workspace workspace) : FencedBaseAgent(workspace), IAgent
{
    public async Task<string> GeneratePrompt(CompileResult compileResult)
    {
        var promptHelper = new PromptHelper(workspace);
        var sb = new StringBuilder();

        var actionsText = GetReducedActionsText();

        await promptHelper.LastResponse(sb);
        await promptHelper.ProjectFiles(sb);
        await promptHelper.CurrentOpenFile(sb);
        await promptHelper.ShowErrorFiles(compileResult, sb);

        var workspaceText = sb.ToString();

        return $@"You are a .NET 10 compiler repair agent.

{actionsText}

{workspaceText}

GOAL
Make the code compile successfully.
Do not change behavior unless required.";
    }

}