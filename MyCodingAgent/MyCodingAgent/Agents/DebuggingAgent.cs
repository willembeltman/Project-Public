using MyCodingAgent.Agents;
using MyCodingAgent.Compile;
using MyCodingAgent.Helpers;
using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.Ollama;
using MyCodingAgent.Tools;
using System.Text;

public class DebuggingAgent(Workspace workspace) : Agent(workspace), IAgent
{
    protected ITool[] tools { get; } =
    [
        new ListAllFiles(workspace),
        new Find(workspace),
        new FindAndReplace(workspace),
        new FindAndReplaceAll(workspace),
        new OpenFile(workspace),
        new CreateOrUpdateFile(workspace),
        new PartialOverwriteFile(workspace),
        new MoveFile(workspace),
        new DeleteFile(workspace),
        new CompileWorkspace(workspace),
        new AskDeveloperForExtraInformation()
    ];

    public async Task<OllamaPrompt> GeneratePrompt(CompileResult compileResult)
    {
        var listAllFilesPrompt = await workspace.GetListAllFilesText();
        var promptHelper = new PromptHelper(workspace);
        List<OllamaPromptMessage> messageList =
        [
            // SYSTEM MESSAGE
            new OllamaPromptMessage(nameof(OllamaAgentRole.system), $@"You are a .NET 10 compiler repair agent."),

            // DIRECTORY OVERVIEW
            new OllamaPromptMessage(nameof(OllamaAgentRole.user), $@"Current workspace files:
{listAllFilesPrompt}")
        ];

        // HISTORY MESSAGES
        AddHistory(messageList, workspace.DebugHistory);

        // ERROR MESSAGE
        var errorView = new StringBuilder();
        await promptHelper.ShowErrorFiles(compileResult, errorView);
        messageList.Add(
            new OllamaPromptMessage(nameof(OllamaAgentRole.user), $@"{errorView}GOAL
Make the code compile successfully.
Do not change behavior unless required."));

        return new OllamaPrompt(
            [.. messageList],
            [.. tools.Select(a => a.ToDto())]);
    }

    public async Task<bool> ProcessResponse(OllamaPrompt prompt, OllamaResponse agentResponse)
    {
        var response = await GetAgentResponseResult(prompt, agentResponse, tools);
        workspace.DebugHistory.Add(response);
        return response.ToolResults.Any(a => a.result.error == false);
    }
}