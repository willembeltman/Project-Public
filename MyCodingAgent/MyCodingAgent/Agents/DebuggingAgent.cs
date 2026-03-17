using MyCodingAgent.Agents;
using MyCodingAgent.Helpers;
using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.ToolCalls;

public class DebuggingAgent(Workspace Workspace, OllamaClient Client) : BaseAgent(Workspace, Client), IAgent
{
    protected override List<PromptResponseResults> History => Workspace.DebugHistory;
    protected override IToolCall[] Tools { get; } =
    [
        new Workspace_Tool(Workspace),
        new AskProjectManager_Tool(Workspace)
    ];

    public async Task<OllamaPrompt> GeneratePrompt(CompileResult compileResult)
    {
        List<OllamaMessage> messageList =
        [
            // SYSTEM MESSAGE
            new OllamaMessage(
                nameof(OllamaAgentRole.system).ToLower(),
                null,
                $@"You are a .NET 10 repair agent.

RULES

- The compiler expects a .csproj, .sln or .slnx file in the ROOT of the workspace, it does not search sub-directories
- You must target .NET 10 for projects. Do not forget!",
                null,
                null),
        ];

        var currentSubTaskMessage = new OllamaMessage(
            nameof(OllamaAgentRole.user).ToLower(),
            null,
            $@"--- CURRENT COMPILATION RESULT ---
{compileResult.Content}
--- END OF COMPILATION RESULT ---

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