using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.ToolCalls;

namespace MyCodingAgent.Agents;

public class PlanningAgent(Workspace Workspace) : BaseAgent(Workspace), IAgent
{
    protected override List<PromptResponseResults> History => Workspace.PlanningHistory;
    protected override ITool[] Tools { get; } =
    [
        new ListAllFiles(Workspace),
        new SearchInAllFiles(Workspace),
        new ShowFile(Workspace),
        new CompileWorkspace(Workspace),
        new ShowAllSubTasks(Workspace),
        new CreateSubTask(Workspace),
        new UpdateSubTask(Workspace),
        new DeleteSubTask(Workspace),
        new SubTasksPlanningIsDone(Workspace),
        new AskDeveloperForExtraInformation(),
        new WorkIsAlreadyDone(Workspace)
    ];

    public async Task<OllamaPrompt> GeneratePrompt(CompileResult compileResult)
    {
        List<OllamaMessage> messageList = 
        [
            // SYSTEM PROMPT
            new OllamaMessage(
                nameof(OllamaAgentRole.system).ToLower(),
                null,
                $@"You are a planning agent inside a .NET 10 development workspace.

Your job is to analyze the developer request and create a subtask plan.

You DO NOT modify code.
You ONLY create and manage subtasks.
You can reply multiple tool_calls.

WORKFLOW

1. Understand the developer request
2. Inspect the workspace if needed (use list_all_files, search, show_file tools)
3. Determine what functionality must be implemented
4. Break the work into clear development subtasks
5. Create subtasks using the create_subtask tool
6. When the full plan is complete call the planning_is_done tool

TASK RULES

- SubTasks must be small and implementable
- SubTasks must describe concrete developer work
- SubTasks must be ordered logically
- Prefer 3-10 subtasks per plan

IMPORTANT

- When you have enough information, STOP investigating and start creating subtasks.
- When the plan is complete you MUST call the tool planning_is_done.

If the requested functionality already exists in the codebase you may call work_is_already_done.",
                null, 
                null),

            // USER ORIGINAL PROMPT
            new OllamaMessage(
                nameof(OllamaAgentRole.user).ToLower(),
                null,
                $@"--- DEVELOPER REQUEST ---
{Workspace.UserPrompt}
--- END OF DEVELOPER REQUEST ---",
                null, 
                null),
        ];

        // CHAT HISTORY
        AddHistoryAndToolCalls(
            messageList, 
            History, 
            [ ..Tools.Select(a => a.ToDto())],
            maxTokens: 4096,
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
    public async Task<bool> ProcessResponse(OllamaPrompt prompt, OllamaResponse agentResponse)
    {
        var response = await GetAgentResponseResult(prompt, agentResponse, Tools);
        History.Add(response);
        return response.ToolCallResults.Any(a => a.result.error == false);
    }
}