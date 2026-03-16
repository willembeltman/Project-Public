using MyCodingAgent.Helpers;
using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.ToolCalls;
using System.Text.Json;

namespace MyCodingAgent.Agents;

public class ProjectManagerAgent(Workspace Workspace) : BaseAgent(Workspace), IAgent
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
        new AnswerCoderQuestion(Workspace),
        new AskDeveloperForExtraInformation()
    ];
    public async Task<OllamaPrompt> GeneratePrompt(CompileResult compileResult)
    {
        if (Workspace.WaitingForProjectManager == null)
            throw new Exception("Geen actieve vraag gevonden voor de Project Manager.");

        List<OllamaMessage> messageList =
        [
            // SYSTEM PROMPT
            new OllamaMessage(
                nameof(OllamaAgentRole.system).ToLower(),
                null,
                $@"You are the Project Manager for a .NET 10 development project. 
Earlier, you created a plan consisting of several subtasks. Now, a Coding Agent is executing one of those tasks and has encountered a blocker or a question.

YOUR MISSION:
1. Analyze the Coding Agent's question in the context of the original project goals and your previous planning.
2. Provide technical clarification, architectural decisions, or missing information.
3. If the question reveals that the original plan was flawed, use 'update_subtask' to refine the plan.
4. Use the 'answer_coder_question' tool to send your definitive answer back to the agent.

CONSTRAINTS:
- You do not write code yourself.
- You provide the guidance so the Coder can continue.
- Use 'list_all_files' or 'show_file' if you need to double-check the current state of the code before answering.

When you have the answer, you MUST call 'answer_coder_question'.",
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
        var questionContent = $@"### INCOMING CODER REQUEST
{Workspace.WaitingForProjectManager.Question}

### CONTEXT: CURRENT SUBTASK DEFINITION
ID: {Workspace.GetCurrentSubTask()?.Id}
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
            maxTokens: 8192,
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