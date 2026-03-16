using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.ToolCalls;
using System.Text.Json;

namespace MyCodingAgent.Agents;

public class CodingAgent(Workspace workspace) : BaseAgent(workspace), IAgent
{
    protected override List<PromptResponseResults> history => workspace.CodingHistory;
    protected override ITool[] tools { get; } =
    [
        new ListAllFiles(workspace),
        new Search(workspace),
        new SearchAndReplace(workspace),
        new SearchAndReplaceAllFiles(workspace),
        new ShowFile(workspace),
        new CreateFile(workspace),
        new UpdateFile(workspace),
        new MoveFile(workspace),
        new DeleteFile(workspace),
        new CompileWorkspace(workspace),
        new AskDeveloperForExtraInformation(),
        new TaskIsFinished(workspace)
    ];

    public async Task<OllamaPrompt> GeneratePrompt(CompileResult compileResult)
    {
        var listAllFilesPrompt = await workspace.GetListAllFilesText();
        List<OllamaMessage> messageList = 
        [
            // SYSTEM PROMPT
            new OllamaMessage(
                nameof(OllamaAgentRole.system).ToLower(),
                null,
                $@"You are an autonomous software engineering agent operating inside a .NET 10 development workspace.

WORKFLOW

1. Understand the request
2. Inspect files if needed
3. Make minimal edits
4. Verify using search, use tools 'open_file' or 'compile_workspace'
5. If the task is completed and the code compiles successfully, call tool 'work_is_done'

IMPORTANT RULE

When the code compiles successfully and the requested functionality is implemented,
you MUST call the 'work_is_done' tool.",
                null, 
                null),

            // USER ORIGINAL PROMPT
            new OllamaMessage(
                nameof(OllamaAgentRole.user).ToLower(),
                null,
                workspace.UserPrompt,
                null, 
                null),
        ];

        // DIRECTORY OVERVIEW
        if (history.Count < 10 && workspace.Files.Count < 80)
        {
            messageList.Add(
                new OllamaMessage(
                    nameof(OllamaAgentRole.user).ToLower(),
                    null,
                    $"Current workspace files:\r\n{listAllFilesPrompt}",
                    null,
                    null));
        }

        var currentTask = workspace.GetCurrentTask();
        var currentTaskMessage = (OllamaMessage?)null;
        var currentTaskMessageJson = string.Empty;
        if (currentTask != null)
        {
            currentTaskMessage = new OllamaMessage(
                nameof(OllamaAgentRole.user).ToLower(),
                null,
                currentTask.Content,
                null,
                null);
            currentTaskMessageJson = JsonSerializer.Serialize(currentTaskMessage, Program.JsonSerializeOptions);
        }

        // CHAT HISTORY
        AddHistoryAndToolCalls(
            messageList, 
            history, 
            [ ..tools.Select(a => a.ToDto())],
            maxTokens: 128000, 
            additionalSizeInBytes: currentTaskMessageJson.Length);

        if (currentTaskMessage != null)
        {
            messageList.Add(currentTaskMessage);
        }

        return new OllamaPrompt(
            [.. messageList],
            [.. tools.Select(a => a.ToDto())]);
    }

    public async Task<bool> ProcessResponse(OllamaPrompt prompt, OllamaResponse agentResponse)
    {
        var response = await GetAgentResponseResult(prompt, agentResponse, tools);
        history.Add(response);
        return response.ToolCallResults.Any(a => a.result.error == false);
    }
}