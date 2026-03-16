using MyCodingAgent.Compile;
using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.Ollama;
using MyCodingAgent.Tools;
using System.ComponentModel;

namespace MyCodingAgent.Agents;

public class CodingAgent(Workspace workspace) : _BaseAgent(workspace), IAgent
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

    // 3. Je hoeft geen complexe JSON meer te bouwen voor tools. 
    // Je kunt gewoon C# methodes annoteren:
    [Description("Lijst alle bestanden in de workspace")]
    public string ListFiles() => string.Join(", ", workspace.Files);

    public async Task<OllamaPrompt> GeneratePrompt(CompileResult compileResult)
    {

        //IChatClient client = new OllamaApiClient(new Uri("http://localhost:11434"), "llama3.1");


        //// 4. Gebruik de ChatClient om alles aan elkaar te knopen
        //var chatOptions = new ChatOptions
        //{
        //    Tools = [AIFunctionFactory.Create(ListFiles)]
        //};

        ////var response = await client.GetResponseAsync("Welke bestanden heb ik?", chatOptions);


        //// De lijst die je geschiedenis bijhoudt
        //List<ChatMessage> chatHistory = new();

        //// Voeg het systeemcommando en de gebruikersvraag toe
        //chatHistory.Add(new ChatMessage(ChatRole.System, "Je bent een .NET agent..."));
        //chatHistory.Add(new ChatMessage(ChatRole.User, workspace.UserPrompt));

        //// De client afvuren
        //var response = await client.GetResponseAsync(chatHistory, chatOptions);

        //// De response (inclusief tool calls!) toevoegen aan de geschiedenis
        //chatHistory.AddRange(response.Messages);




        var history = workspace.CodingHistory;
        var listAllFilesPrompt = await workspace.GetListAllFilesText();
        List<OllamaMessage> messageList = 
        [
            // SYSTEM PROMPT
            new OllamaMessage(
                nameof(OllamaAgentRole.system).ToLower(),
                null,
                $@"You are an autonomous software engineering agent operating inside a .NET 10 development workspace.

WORKFLOW

1. Understand the request
2. Inspect files if needed
3. Make minimal edits
4. Verify using search, use tools 'open_file' or 'compile_workspace'
5. If the task is completed and the code compiles successfully, call tool 'work_is_done'

IMPORTANT RULE

When the code compiles successfully and the requested functionality is implemented,
you MUST call the 'work_is_done' tool.",
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

        // DIRECTORY OVERVIEW
        if (history.Count < 10 && workspace.Files.Count < 80)
        {
            messageList.Add(
                new OllamaMessage(
                    nameof(OllamaAgentRole.user).ToLower(),
                    null,
                    $"Current workspace files:\r\n{listAllFilesPrompt}",
                    null,
                    null));
        }

        // CHAT HISTORY
        AddHistory(messageList, history, 
            maxLongDesciptionPrompt: 10, 
            maxLongDescriptionResponse: 10,
            maxHistory: 10);

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