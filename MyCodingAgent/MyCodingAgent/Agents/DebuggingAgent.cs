using MyCodingAgent.Agents;
using MyCodingAgent.Compile;
using MyCodingAgent.Helpers;
using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.Ollama;
using MyCodingAgent.Tools;
using System.Text;

public class DebuggingAgent(Workspace workspace) : _BaseAgent(workspace), IAgent
{
    protected ITool[] tools { get; } =
    [
        new Find(workspace),
        new FindAndReplace(workspace),
        new FindAndReplaceAll(workspace),
        new OpenFile(workspace),
        new CreateOrUpdateFile(workspace),
        new PartialOverwriteFile(workspace),
        new MoveFile(workspace),
        new DeleteFile(workspace),
        new AskDeveloperForExtraInformation()
    ];

    public async Task<OllamaPrompt> GeneratePrompt(CompileResult compileResult)
    {
        var listAllFilesPrompt = await workspace.GetListAllFilesText();
        var promptHelper = new PromptHelper(workspace);
        List<OllamaMessage> messageList =
        [
            // SYSTEM MESSAGE
            new OllamaMessage(
                nameof(OllamaAgentRole.system).ToLower(),
                null,
                $@"You are a .NET 10 repair agent.",
                null,
                null),

            // DIRECTORY OVERVIEW
            new OllamaMessage(
                nameof(OllamaAgentRole.user).ToLower(),
                null,
                $@"Current workspace files:
{listAllFilesPrompt}",
                null,
                null)
        ];

        // HISTORY MESSAGES
        AddHistory(messageList, workspace.DebugHistory, 
            maxLongDesciptionPrompt: 10,
            maxLongDescriptionResponse: 10,
            maxHistory: 10);

        // ERROR MESSAGE
        var errorView = new StringBuilder();
        await promptHelper.ShowErrorFiles(compileResult, errorView);
        messageList.Add(
            new OllamaMessage(
                nameof(OllamaAgentRole.user).ToLower(),
                null,
                $@"{errorView}GOAL
Make the code compile successfully.
Do not change behavior unless required.",
                null,
                null));

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