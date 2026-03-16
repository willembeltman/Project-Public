using MyCodingAgent.Compile;
using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.Ollama;
using MyCodingAgent.Tools;
using System.Text.Json;

namespace MyCodingAgent.Agents;

public class CodingAgent(Workspace workspace) : Agent(workspace), IAgent
{
    protected ITool[] tools { get; } =
    [
        new ListAllFiles(workspace),
        new Find(workspace),
        new FindAndReplace(workspace),
        new FindAndReplaceAll(workspace),
        new OpenFile(workspace),
        new CreateOrUpdateFile(workspace),
        new PartialOverwriteFile(workspace),
        new MoveFile(workspace),
        new DeleteFile(workspace),
        new CompileWorkspace(workspace),
        new AskDeveloperForExtraInformation(),
        new WorkIsDone(workspace)
    ];

    public async Task<OllamaPrompt> GeneratePrompt(CompileResult compileResult)
    {
        var history = workspace.CodingHistory;
        var listAllFilesPrompt = await workspace.GetListAllFilesText();
        List<OllamaPromptMessage> messageList = 
        [
            // SYSTEM PROMPT
            new OllamaPromptMessage(
                nameof(OllamaAgentRole.system),
                $@"You are an autonomous software engineering agent operating inside a .NET 10 development workspace.

WORKFLOW

1. Understand the request
2. Inspect files if needed
3. Make minimal edits
4. Verify using search, use tools 'open_file' or 'compile_workspace'
5. If the task is completed and the code compiles successfully, call tool 'work_is_done'

IMPORTANT RULE

When the code compiles successfully and the requested functionality is implemented,
you MUST call the 'work_is_done' tool."),
            // USER ORIGINAL PROMPT
            new OllamaPromptMessage(nameof(OllamaAgentRole.user), workspace.UserPrompt),
        ];

        // DIRECTORY OVERVIEW
        if (history.Count < 10 && workspace.Files.Count < 80)
        {
            messageList.Add(new OllamaPromptMessage(nameof(OllamaAgentRole.user), $"Current workspace files:\r\n{listAllFilesPrompt}"));
        }

        // CHAT HISTORY
        AddHistory(messageList, history, 
            maxLongDesciptionPrompt: 4, 
            maxLongDescriptionResponse: 4,
            maxHistory: 12);

        return new OllamaPrompt(
            [.. messageList],
            [.. tools.Select(a => a.ToDto())]);
    }

    public async Task<bool> ProcessResponse(OllamaPrompt prompt, OllamaResponse agentResponse)
    {
        var response = await GetAgentResponseResult(prompt, agentResponse, tools);
        workspace.CodingHistory.Add(response);
        return response.ToolResults.Any(a => a.result.error == false);
    }
}