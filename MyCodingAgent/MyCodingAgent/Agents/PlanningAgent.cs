using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.ToolCalls;

namespace MyCodingAgent.Agents;

public class PlanningAgent(Workspace workspace) : BaseAgent(workspace), IAgent
{
    protected override List<PromptResponseResults> history => workspace.PlanningHistory;
    protected override ITool[] tools { get; } =
    [
        new ListAllFiles(workspace),
        new Search(workspace),
        new ShowFile(workspace),
        new CompileWorkspace(workspace),
        new CreateTask(workspace),
        new UpdateTask(workspace),
        new DeleteTask(workspace),
        new AskDeveloperForExtraInformation(),
        new PlanningIsDone(workspace),
        new WorkIsAlreadyDone(workspace)
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
                $@"You are a planning agent inside a .NET 10 development workspace.

Your job is to analyze the developer request and create a task plan.

You DO NOT modify code.
You ONLY create and manage tasks.

WORKFLOW

1. Understand the developer request
2. Inspect the workspace if needed (use list_all_files, search, show_file)
3. Determine what functionality must be implemented
4. Break the work into clear development tasks
5. Create tasks using the create_task tool
6. When the full plan is complete call the planning_is_done tool

TASK RULES

- Tasks must be small and implementable
- Tasks must describe concrete developer work
- Tasks must be ordered logically
- Prefer 3-10 tasks per plan

IMPORTANT

When the plan is complete you MUST call the tool planning_is_done.

If the requested functionality already exists in the codebase you may call work_is_already_done.

Example plan:

Task 1
Implement API endpoint for creating users

Task 2
Add database entity for User

Task 3
Add validation logic for user input

Task 4
Add integration tests",
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

        //// DIRECTORY OVERVIEW
        ////if (history.Count < 10 && workspace.Files.Count < 80)
        //{
        //    messageList.Add(
        //        new OllamaMessage(
        //            nameof(OllamaAgentRole.user).ToLower(),
        //            null,
        //            $"Current workspace files:\r\n{listAllFilesPrompt}",
        //            null,
        //            null));
        //}

        // CHAT HISTORY
        AddHistoryAndToolCalls(
            messageList, 
            history, 
            [ ..tools.Select(a => a.ToDto())],
            maxTokens: 128000, 
            additionalSizeInBytes: 0);

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