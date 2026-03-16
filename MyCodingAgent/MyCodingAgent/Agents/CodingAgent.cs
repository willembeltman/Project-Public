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
        new SearchAllFiles(workspace),
        new SearchAndReplace(workspace),
        new SearchAndReplaceAllFiles(workspace),
        new ShowFile(workspace),
        new ShowFileWithLineNumbers(workspace),
        new CreateFile(workspace),
        new UpdateFile(workspace),
        new MoveFile(workspace),
        new DeleteFile(workspace),
        new CompileWorkspace(workspace),
        new AskDeveloperForExtraInformation(),
        new SubTaskIsFinished(workspace)
    ];

    public async Task<OllamaPrompt> GeneratePrompt(CompileResult compileResult)
    {
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
5. If the subtask is completed and the code compiles successfully, call tool 'work_is_done'

IMPORTANT RULE

When the code compiles successfully and the requested functionality is implemented,
you MUST call the 'work_is_done' tool.",
                null,
                null),

            //// USER ORIGINAL PROMPT
            //new OllamaMessage(
            //    nameof(OllamaAgentRole.user).ToLower(),
            //    null,
            //    workspace.UserPrompt,
            //    null,
            //    null),
        ];

        //// DIRECTORY OVERVIEW
        //if (history.Count < 10 && workspace.Files.Count < 80)
        //{
        //    var listAllFilesPrompt = await workspace.GetListAllFilesText();
        //    messageList.Add(
        //        new OllamaMessage(
        //            nameof(OllamaAgentRole.user).ToLower(),
        //            null,
        //            $"Current workspace files:\r\n{listAllFilesPrompt}",
        //            null,
        //            null));
        //}

        var currentSubTask = workspace.GetCurrentSubTask();
        var currentSubTaskMessage = (OllamaMessage?)null;
        var currentSubTaskMessageJson = string.Empty;
        if (currentSubTask != null)
        {
            currentSubTaskMessage = new OllamaMessage(
                nameof(OllamaAgentRole.user).ToLower(),
                null,
                currentSubTask.Content,
                null,
                null);
            currentSubTaskMessageJson = JsonSerializer.Serialize(currentSubTaskMessage, Program.JsonSerializeOptions);
            messageList.Add(currentSubTaskMessage);
        }

        // CHAT HISTORY
        AddHistoryAndToolCalls(
            messageList,
            history,
            [.. tools.Select(a => a.ToDto())],
            maxTokens: 8192,
            additionalSizeInBytes: currentSubTaskMessageJson.Length);

        //if (currentSubTaskMessage != null)
        //{
        //    messageList.Add(currentSubTaskMessage);
        //}

        return new OllamaPrompt(
            [.. messageList],
            [.. tools.Select(a => a.ToDto())]);
    }

    public Task<bool> ProcessResponse(OllamaPrompt prompt, OllamaResponse agentResponse)
        => ProcessResponse(prompt, agentResponse, true);
    public async Task<bool> ProcessResponse(OllamaPrompt prompt, OllamaResponse agentResponse, bool save = true)
    {
        var response = await GetAgentResponseResult(prompt, agentResponse, tools);
        if (save) history.Add(response);
        return response.ToolCallResults.Any(a => a.result.error == false);
    }
}