using MyCodingAgent.Agents;
using MyCodingAgent.Helpers;
using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.ToolCalls;
using System.Text;
using System.Text.Json;

public class DebuggingAgent(Workspace workspace) : BaseAgent(workspace), IAgent
{
    protected override List<PromptResponseResults> history => workspace.DebugHistory;
    protected override ITool[] tools { get; } =
    [
        new Search(workspace),
        new SearchAndReplace(workspace),
        new SearchAndReplaceAllFiles(workspace),
        new ShowFile(workspace),
        new CreateFile(workspace),
        new UpdateFile(workspace),
        new MoveFile(workspace),
        new DeleteFile(workspace),
        new AskDeveloperForExtraInformation()
    ];

    public async Task<OllamaPrompt> GeneratePrompt(CompileResult compileResult)
    {
        var listAllFilesPrompt = await workspace.GetListAllFilesText();
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

        var currentTask = workspace.GetCurrentTask();
        if (currentTask != null)
        {
            var currentTaskMessage = new OllamaMessage(
                nameof(OllamaAgentRole.user).ToLower(),
                null,
                currentTask.Content,
                null,
                null);
            messageList.Add(currentTaskMessage);
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
        var errorMessageJson = JsonSerializer.Serialize(errorMessage, Program.JsonSerializeOptions);
        AddHistoryAndToolCalls(messageList, history, [.. tools.Select(a => a.ToDto())], 128000, errorMessageJson.Length);

        // ERROR MESSAGE
        messageList.Add(errorMessage);

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

            var file = workspace.GetFile(relativePath);
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