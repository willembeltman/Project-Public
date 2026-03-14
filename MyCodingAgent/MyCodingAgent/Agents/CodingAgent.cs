using MyCodingAgent.Compile;
using MyCodingAgent.Models;
using MyCodingAgent.Helpers;
using System.Text;
using MyCodingAgent.BaseAgents;
using MyCodingAgent.Interfaces;

namespace MyCodingAgent.Agents;

public class CodingAgent(Workspace workspace) : FencedBaseAgent(workspace), IAgent
{
    public async Task<string> GeneratePrompt(CompileResult compileResult)
    {
        var promptHelper = new PromptHelper(workspace);
        var sb = new StringBuilder();

        await promptHelper.LastResponse(sb);
        await promptHelper.ProjectFiles(sb);
        await promptHelper.CurrentOpenFile(sb);
        await promptHelper.SearchResults(sb);
        await promptHelper.CurrentPrompt(sb);
        await promptHelper.SavedTasks(sb);

        var workspaceText = sb.ToString();
        var actionsText = GetActionsText();

        return $@"You are an autonomous software engineering agent operating inside a .NET 10 development workspace.
{actionsText}

{workspaceText}

WORKFLOW

1. Understand the request
2. Inspect files if needed
3. Make minimal edits
4. Verify using search or open_file

Always prefer small incremental steps.";
    }
}