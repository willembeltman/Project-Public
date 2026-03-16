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
        new SearchInAllFiles(Workspace),
        new SearchAndReplace(Workspace),
        new SearchAndReplaceInAllFiles(Workspace),
        new ShowFile(Workspace),
        new CreateFile(Workspace),
        new ReplaceLinesInFile(Workspace),
        new MoveFile(Workspace),
        new DeleteFile(Workspace),
        new AskDeveloperForExtraInformation()
    ];

    public async Task<OllamaPrompt> GeneratePrompt(CompileResult compileResult)
    {
        //var listAllFilesPrompt = await workspace.GetListAllFilesText();
        List<OllamaMessage> messageList =
        [
            // SYSTEM MESSAGE
            new OllamaMessage(
                nameof(OllamaAgentRole.system).ToLower(),
                null,
                $@"You are a .NET 10 repair agent.",
                null,
                null),

//            // DIRECTORY OVERVIEW
//            new OllamaMessage(
//                nameof(OllamaAgentRole.user).ToLower(),
//                null,
//                $@"Current workspace files:
//{listAllFilesPrompt}",
//                null,
//                null)
        ];

        var currentSubTask = Workspace.GetCurrentSubTask();
        if (currentSubTask != null)
        {
            var currentSubTaskMessage = new OllamaMessage(
                nameof(OllamaAgentRole.user).ToLower(),
                null,
                currentSubTask.Content,
                null,
                null);
            messageList.Add(currentSubTaskMessage);
        }

        // HISTORY MESSAGES
        
        var errorView = await ShowErrorFiles(compileResult);
        var errorMessage =
            new OllamaMessage(
                nameof(OllamaAgentRole.user).ToLower(),
                null,
                $@"{errorView}GOAL
Make the code compile successfully.
Do not change behavior unless required.",
                null,
                null);
        var errorMessageJson = JsonSerializer.Serialize(errorMessage, DefaultJsonSerializerOptions.JsonSerializeOptionsIndented);
        AddHistoryAndToolCalls(
            messageList, 
            History, 
            [.. Tools.Select(a => a.ToDto())], 
            maxTokens: 8192, 
            additionalSizeInBytes: errorMessageJson.Length);

        // ERROR MESSAGE
        messageList.Add(errorMessage);

        return new OllamaPrompt(
            [.. messageList],
            [.. Tools.Select(a => a.ToDto())]);
    }

    public async Task<bool> ProcessResponse(OllamaPrompt prompt, OllamaResponse agentResponse)
    {
        var response = await GetAgentResponseResult(prompt, agentResponse, Tools);
        History.Add(response);
        return response.ToolCallResults.Any(a => a.result.error == false);
    }

    private async Task<string> ShowErrorFiles(CompileResult compileResult)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"ERRORS (GROUPED BY FILES)");
        sb.AppendLine();
        foreach (var fileGroup in compileResult.Errors.GroupBy(a => a.File))
        {
            var relativePath = fileGroup.Key;
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                sb.AppendLine("FILE: <null>");
                foreach (var error in fileGroup)
                {
                    sb.AppendLine(error.FullError.TrimEnd('\n').TrimEnd('\r'));
                }
                sb.AppendLine();
                continue;
            }

            var file = Workspace.GetFile(relativePath);
            if (file != null)
            {
                sb.AppendLine($"FILE: {relativePath}");
                sb.AppendLine("CODE");
                var fileContent = await file.GetFileContent();
                foreach (var line in fileContent.GetLines())
                {
                    sb.AppendLine($"{line.lineNumber,3}|{line.content}");
                }
                sb.AppendLine();
                sb.AppendLine("ERRORS");
                foreach (var error in fileGroup)
                {
                    sb.AppendLine(error.FullError.TrimEnd('\n').TrimEnd('\r'));
                }
                sb.AppendLine();
            }
        }

        return sb.ToString();
    }
}