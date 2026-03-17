using MyCodingAgent.Helpers;
using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.ToolCalls;
using System.Text.Json;

namespace MyCodingAgent.Agents;

public class CodingAgent(Workspace Workspace, OllamaClient Client) : BaseAgent(Workspace, Client), IAgent
{
    protected override List<PromptResponseResults> History => Workspace.CodingHistory;
    protected override IToolCall[] Tools { get; } =
    [
        new Workspace_Tool(Workspace),
        new Ask_CodingAgent_To_ProjectManager_Tool(Workspace),
        new CurrentSubTaskIsFinished_Tool(Workspace)
    ];
    public Workspace_Tool WorkspaceTool
        => (Tools.First(a => a is Workspace_Tool) as Workspace_Tool)!;
    public Ask_CodingAgent_To_ProjectManager_Tool AskProjectManagerTool
        => (Tools.First(a => a is Ask_CodingAgent_To_ProjectManager_Tool) as Ask_CodingAgent_To_ProjectManager_Tool)!;
    public CurrentSubTaskIsFinished_Tool CurrentSubTaskIsFinishedTool
        => (Tools.First(a => a is CurrentSubTaskIsFinished_Tool) as CurrentSubTaskIsFinished_Tool)!;

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
You must complete this subtask by applying all required changes.

WORKFLOW

1. Understand the request.
2. Inspect files before changing them.
3. Make the smallest possible change to achieve the goal. Do not rewrite entire files unless absolutely necessary.
4. Ask project manager for advice using '{AskProjectManagerTool.Name}' tool, if you are unsure.
5. Compile the project using the '{WorkspaceTool.Name}' tool.
6. Fix any compilation warnings if they occur.
7. Verify your edits using 'diff_with_original' action of the '{WorkspaceTool.Name}' tool.
8. If everything is correct, call '{CurrentSubTaskIsFinishedTool.Name}'.

RULES

- The compiler expects a .csproj, .sln or .slnx file in the root of the workspace.
- You can compile using 'path' parameter for specific projects.

IMPORTANT RULES

- When the code compiles successfully and the requested functionality is implemented,
  you MUST call the '{CurrentSubTaskIsFinishedTool.Name}' tool.
- You MUST read a file using the '{WorkspaceTool.Name}' tool before modifying it.
- You MUST target .NET 10 for projects. Do not forget!",
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
    public Task<bool> ProcessResponse(OllamaPrompt prompt, OllamaResponse agentResponse)
        => ProcessResponse(prompt, agentResponse, true);
    public async Task<bool> ProcessResponse(OllamaPrompt prompt, OllamaResponse agentResponse, bool save)
    {
        var response = await GetAgentResponseResult(prompt, agentResponse, Tools);
        if (save) History.Add(response);
        return response.ToolCallResults.Any(a => a.result.error == false);
    }
}