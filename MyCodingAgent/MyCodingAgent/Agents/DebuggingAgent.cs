using MyCodingAgent.Agents;
using MyCodingAgent.Helpers;
using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.ToolCalls;
using System.Text;
using System.Text.Json;

public class DebuggingAgent(Workspace Workspace) : BaseAgent(Workspace), IAgent
{
    protected override List<PromptResponseResults> History => Workspace.DebugHistory;
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
        new AskDeveloperForExtraInformation()
    ];

    public async Task<OllamaPrompt> GeneratePrompt(CompileResult compileResult)
    {
        List<OllamaMessage> messageList =
        [
            // SYSTEM MESSAGE
            new OllamaMessage(
                nameof(OllamaAgentRole.system).ToLower(),
                null,
                $@"You are a .NET 10 repair agent.",
                null,
                null),
        ];

        var currentSubTask = Workspace.GetCurrentSubTask();
        if (currentSubTask != null)
        {
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
        }

        //        // HISTORY MESSAGES        
        //        var errorView = await ShowErrorFiles(compileResult);
        //        var errorMessage =
        //            new OllamaMessage(
        //                nameof(OllamaAgentRole.user).ToLower(),
        //                null,
        //                $@"{errorView}GOAL
        //Make the code compile successfully.
        //Do not change behavior unless required.",
        //                null,
        //                null);
        //        var errorMessageJson = JsonSerializer.Serialize(errorMessage, DefaultJsonSerializerOptions.JsonSerializeOptionsIndented);

        AddHistoryAndToolCalls(
            messageList,
            History,
            [.. Tools.Select(a => a.ToDto())],
            maxTokens: 8192,
            additionalSizeInBytes: 0);//errorMessageJson.Length);

        // ERROR MESSAGE
        //messageList.Add(errorMessage);

        return new OllamaPrompt(
            [.. messageList],
            [.. Tools.Select(a => a.ToDto())]);
    }
    //private async Task<string> ShowErrorFiles(CompileResult compileResult)
    //{
    //    StringBuilder sb = new StringBuilder();

    //    sb.AppendLine($"ERRORS (GROUPED BY FILES)");
    //    sb.AppendLine();
    //    foreach (var fileGroup in compileResult.Errors.GroupBy(a => a.File))
    //    {
    //        var relativePath = fileGroup.Key;
    //        if (string.IsNullOrWhiteSpace(relativePath))
    //        {
    //            sb.AppendLine("FILE: <null>");
    //            foreach (var error in fileGroup)
    //            {
    //                sb.AppendLine(error.FullError.TrimEnd('\n').TrimEnd('\r'));
    //            }
    //            sb.AppendLine();
    //            continue;
    //        }

    //        var file = Workspace.GetFile(relativePath);
    //        if (file != null)
    //        {
    //            sb.AppendLine($"FILE: {relativePath}");
    //            sb.AppendLine("CODE");
    //            var fileContent = await file.GetFileContent();
    //            foreach (var line in fileContent.GetLines())
    //            {
    //                sb.AppendLine($"{line.lineNumber,3}|{line.content}");
    //            }
    //            sb.AppendLine();
    //            sb.AppendLine("ERRORS");
    //            foreach (var error in fileGroup)
    //            {
    //                sb.AppendLine(error.FullError.TrimEnd('\n').TrimEnd('\r'));
    //            }
    //            sb.AppendLine();
    //        }
    //    }

    //    return sb.ToString();
    //}

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