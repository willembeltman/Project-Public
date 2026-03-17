using MyCodingAgent.Helpers;
using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.ToolCalls;
using System.Text.Json;

namespace MyCodingAgent.Agents;

public class ProjectManagerAgentForCodingAgent(Workspace Workspace, OllamaClient Client) : BaseAgent(Workspace, Client), IAgent
{
    protected override List<PromptResponseResults> History => Workspace.PlanningHistory;
    protected override IToolCall[] Tools { get; } =
    [
        new WorkspaceReadonly_Tool(Workspace),
        new SubTasks_Tool(Workspace),
        new Answer_ProjectManagerAgent_To_CoderAgent_Tool(Workspace),
        new Ask_HumanDeveloper_Tool()
    ];

    public Answer_ProjectManagerAgent_To_CoderAgent_Tool Answer_CoderAgent_Tool 
        => (Tools.First(a => a is Answer_ProjectManagerAgent_To_CoderAgent_Tool) as Answer_ProjectManagerAgent_To_CoderAgent_Tool)!;
    public SubTasks_Tool SubTasks_Tool
        => (Tools.First(a => a is SubTasks_Tool) as SubTasks_Tool)!;
    public WorkspaceReadonly_Tool WorkspaceReadonly_Tool
        => (Tools.First(a => a is WorkspaceReadonly_Tool) as WorkspaceReadonly_Tool)!;

    public async Task<OllamaPrompt> GeneratePrompt(CompileResult compileResult)
    {
        if (Workspace.CodingAgent_WaitingFor_ProjectManagerAgent_Answer == null)
            throw new Exception("No active job found for Project Manager.");

        List<OllamaMessage> messageList =
        [
            // SYSTEM PROMPT
            new OllamaMessage(
                nameof(OllamaAgentRole.system).ToLower(),
                null,
                $@"You are the Project Manager for a .NET 10 development project. 
Earlier, you created a plan consisting of several subtasks. Now, a Coder Agent is executing one of those tasks and has encountered a blocker or a question.

YOUR MISSION:
1. Analyze the Coder Agent's question in the context of the original project goals and your previous planning.
2. Provide technical clarification, architectural decisions, or missing information.
3. If the question reveals that the original plan was flawed, use '{SubTasks_Tool.Name}' to refine the plan.
4. Use the '{Answer_CoderAgent_Tool.Name}' tool to send your definitive answer back to the agent.

CONSTRAINTS:
- You do not write code yourself.
- You provide the guidance so the Coder Agent can continue.
- Use '{WorkspaceReadonly_Tool.Name}' tools, if you need to double-check the current state of the code before answering.

RULES:
- You must target .NET 10 for projects. Do not forget!
- Only if it is really unclear you can ask the developer for extra information

When you have the answer, you MUST call '{Answer_CoderAgent_Tool.Name}'.",
                null,
                null),

            // USER ORIGINAL PROMPT (Het grote doel)
            new OllamaMessage(
                nameof(OllamaAgentRole.user).ToLower(),
                null,
                $"Original Project Goal: {Workspace.UserPrompt}",
                null,
                null),
        ];

        // De vraag van de Coder verpakken we als een specifieke User-message
        var questionContent = $@"### INCOMING CODER AGENT REQUEST
{Workspace.CodingAgent_WaitingFor_ProjectManagerAgent_Answer.Question}

### CONTEXT: CURRENT SUBTASK DEFINITION
{Workspace.GetCurrentSubTask()?.Content}

### GUIDANCE
Please analyze the request above against the subtask definition and provide the necessary information to unblock the Coder.";

        var question = new OllamaMessage(
            nameof(OllamaAgentRole.user).ToLower(),
            null,
            questionContent,
            null,
            null);

        var questionJson = JsonSerializer.Serialize(question, DefaultJsonSerializerOptions.JsonSerializeOptionsIndented);

        // CHAT HISTORY (Hier zit je create_subtask historie in)
        AddHistoryAndToolCalls(
            messageList,
            History,
            [.. Tools.Select(a => a.ToDto())],
            maxTokens: 4096,
            additionalSizeInBytes: questionJson.Length);

        // Voeg de actuele vraag als laatste toe zodat deze de meeste prioriteit heeft
        messageList.Add(question);

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