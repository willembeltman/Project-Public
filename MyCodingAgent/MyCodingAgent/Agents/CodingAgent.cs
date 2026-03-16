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
                null)
        ];

        var currentSubTask = Workspace.GetCurrentSubTask();
        if (currentSubTask != null)
        {
            var currentSubTaskMessage = new OllamaMessage(
                nameof(OllamaAgentRole.user).ToLower(),
                null,
                $@"--- CURRENT SUBTASK ---
{currentSubTask.Content}
--- END OF SUBTASK ---",
                null,
                null);
            messageList.Add(currentSubTaskMessage);
        }

        // CHAT HISTORY
        AddHistoryAndToolCalls(
            messageList,
            History,
            [.. Tools.Select(a => a.ToDto())],
            maxTokens: 8192,
            additionalSizeInBytes: 0);

        return new OllamaPrompt(
            [.. messageList],
            [.. Tools.Select(a => a.ToDto())]);
    }

    /// <summary>
    /// Processes the agent's response to the specified prompt and determines whether any tool call completed without error.
    /// </summary>
    /// <param name="prompt">The prompt that was sent to the agent. This provides the context or question for which the response is being
    /// processed.</param>
    /// <param name="agentResponse">The response object returned by the agent, containing the results to be evaluated.</param>
    /// <returns>if there was any tool call, if not this indicates maybe a different agent should continue</returns>
    public Task<bool> ProcessResponse(OllamaPrompt prompt, OllamaResponse agentResponse)
        => ProcessResponse(prompt, agentResponse, true);
    public async Task<bool> ProcessResponse(OllamaPrompt prompt, OllamaResponse agentResponse, bool save)
    {
        var response = await GetAgentResponseResult(prompt, agentResponse, Tools);
        if (save) History.Add(response);
        return response.ToolCallResults.Any(a => a.result.error == false);
    }
}