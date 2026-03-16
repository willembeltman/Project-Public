using MyCodingAgent.Helpers;
using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.ToolCalls;
using System.Text.Json;

namespace MyCodingAgent.Agents;

public class CodingAgent(Workspace Workspace) : BaseAgent(Workspace), IAgent
{
    protected override List<PromptResponseResults> History => Workspace.CodingHistory;
    protected override ITool[] Tools { get; } =
    [
        new ListAllFiles(Workspace),
        new SearchInAllFiles(Workspace),
        new SearchAndReplace(Workspace),
        new SearchAndReplaceInAllFiles(Workspace),
        new ShowFile(Workspace),
        new ShowFileWithLineNumbers(Workspace),
        new CreateFile(Workspace),
        new MoveFile(Workspace),
        new DeleteFile(Workspace),
        new ReplaceLinesInFile(Workspace),
        new InsertLinesInFile(Workspace),
        new CompileWorkspace(Workspace),
        new AskProjectManagerForExtraInformation(Workspace),
        new AskDeveloperForExtraInformation(),
        new SubTaskIsFinished(Workspace)
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
You have been assigned a subtask for the project in your workspace.
You must complete this subtask by applying all changes

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

        var currentSubTask = Workspace.GetCurrentSubTask();
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
            currentSubTaskMessageJson = JsonSerializer.Serialize(currentSubTaskMessage, DefaultJsonSerializerOptions.JsonSerializeOptionsIndented);
            messageList.Add(currentSubTaskMessage);
        }

        // CHAT HISTORY
        AddHistoryAndToolCalls(
            messageList,
            History,
            [.. Tools.Select(a => a.ToDto())],
            maxTokens: 8192,
            additionalSizeInBytes: currentSubTaskMessageJson.Length);

        //if (currentSubTaskMessage != null)
        //{
        //    messageList.Add(currentSubTaskMessage);
        //}

        return new OllamaPrompt(
            [.. messageList],
            [.. Tools.Select(a => a.ToDto())]);
    }

    public Task<bool> ProcessResponse(OllamaPrompt prompt, OllamaResponse agentResponse)
        => ProcessResponse(prompt, agentResponse, true);
    public async Task<bool> ProcessResponse(OllamaPrompt prompt, OllamaResponse agentResponse, bool save = true)
    {
        var response = await GetAgentResponseResult(prompt, agentResponse, Tools);
        if (save) History.Add(response);
        return response.ToolCallResults.Any(a => a.result.error == false);
    }
}