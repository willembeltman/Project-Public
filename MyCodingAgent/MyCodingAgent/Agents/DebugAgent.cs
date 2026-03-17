using MyCodingAgent.Agents;
using MyCodingAgent.Helpers;
using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.ToolCalls;

public class DebugAgent(Workspace workspace, OllamaClient client) : BaseAgent(workspace, client), IAgent
{
    protected override List<PromptResponseResults> History => Workspace.DebugHistory;
    protected override IToolCall[] Tools { get; } =
    [
        new Workspace_Tool(workspace),
        new DebugAgentIsDone_Tool(workspace),
        new Ask_DebugAgent_To_CoderAgent_Tool(workspace),
        new Ask_DebugAgent_To_ProjectManager_Tool(workspace)
    ];
    public Workspace_Tool WorkspaceTool
        => (Tools.First(a => a is Workspace_Tool) as Workspace_Tool)!;
    public DebugAgentIsDone_Tool DebugAgentIsDoneTool
        => (Tools.First(a => a is DebugAgentIsDone_Tool) as DebugAgentIsDone_Tool)!;
    public Ask_DebugAgent_To_CoderAgent_Tool AskCoderAgentTool
        => (Tools.First(a => a is Ask_DebugAgent_To_CoderAgent_Tool) as Ask_DebugAgent_To_CoderAgent_Tool)!;
    public Ask_DebugAgent_To_ProjectManager_Tool AskProjectManagerTool
        => (Tools.First(a => a is Ask_DebugAgent_To_ProjectManager_Tool) as Ask_DebugAgent_To_ProjectManager_Tool)!;

    public async Task<OllamaPrompt> GeneratePrompt(CompileResult compileResult)
    {
        List<OllamaMessage> messageList =
        [
            // SYSTEM MESSAGE
            new OllamaMessage(
                nameof(OllamaAgentRole.system).ToLower(),
                null,
                $@"You are a .NET 10 repair agent.

GOAL
Fix compilation errors with minimal changes.

WORKFLOW
1. Analyze the error.
2. Find the root cause.
3. Read relevant files using '{WorkspaceTool.Name}'.
4. Apply the smallest possible fix.
5. Recompile using '{WorkspaceTool.Name}'.
6. Repeat until it compiles.
7. Call '{DebugAgentIsDoneTool.Name}' when done.

RULES
- A .csproj, .sln, or .slnx must exist in the ROOT (no sub-directory search).
- Always read a file before modifying it.
- Do not overwrite entire files unless necessary.
- Use '{WorkspaceTool.Name}' for ALL file operations.
- Target .NET 10 only.

IF UNSURE
Use '{AskCoderAgentTool.Name}' or '{AskProjectManagerTool.Name}'. Do not guess.",
                null,
                null),
        ];

        var currentSubTaskMessage = new OllamaMessage(
            nameof(OllamaAgentRole.user).ToLower(),
            null,
            $@"--- CURRENT COMPILATION RESULT ---
{compileResult.Content}
--- END OF COMPILATION RESULT ---
Note: this is up-to-date.

GOAL
Make the code compile successfully.
Do not change behavior unless required.",
            null,
            null);
        messageList.Add(currentSubTaskMessage);

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
    public async Task<bool> ProcessResponse(OllamaPrompt prompt, OllamaResponse agentResponse)
    {
        var response = await GetAgentResponseResult(prompt, agentResponse, Tools);
        History.Add(response);
        return response.ToolCallResults.Any(a => a.result.error == false);
    }
}