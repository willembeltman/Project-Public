using MyCodingAgent.Compile;
using MyCodingAgent.Models;
using MyCodingAgent.Helpers;
using System.Text;
using MyCodingAgent.Interfaces;
using MyCodingAgent.BaseAgents;
using MyCodingAgent.Ollama;

public class DebuggingAgent(Workspace workspace) : JsonBaseAgent(workspace), IAgent
{
    public async Task<OllamaPrompt> GeneratePrompt(CompileResult compileResult)
    {
        string tools = CreateToolsString();

        var promptHelper = new PromptHelper(workspace);
        var sb = new StringBuilder();

        var actions = GetReducedActionsText();

        //await promptHelper.LastResponse(sb);
        await promptHelper.ProjectFiles(sb);
        await promptHelper.CurrentOpenFile(sb);
        await promptHelper.ShowErrorFiles(compileResult, sb);

        var workspaceText = sb.ToString();

        var prompt = $@"You are a .NET 10 compiler repair agent.

{actions}

{workspaceText}

GOAL
Make the code compile successfully.
Do not change behavior unless required.";

        return new OllamaPrompt([new OllamaMessage(nameof(OllamaAgentRole.user), prompt)], tools);
    }

}