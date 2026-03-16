using MyCodingAgent.Agents;
using MyCodingAgent.Helpers;
using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

internal class Program : IDisposable
{
    readonly CancellationTokenSource Cts;
    readonly HashSet<OllamaMessage> ShownMessages;
    readonly OllamaClient LlmService;

    private Program()
    {
        Cts = new CancellationTokenSource();
        ShownMessages = new HashSet<OllamaMessage>();
        LlmService = new OllamaClient();
    }

    private async Task StartAsync()
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("MyCodingAgent v0.001, created by Willem-Jan Beltman");
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Loading workspace, please wait...");

        var workspaceDirectory = Path.Combine(Environment.CurrentDirectory, "Source");
        var workspace = await Workspace.TryLoad(workspaceDirectory);
        if (workspace == null || workspace.WorkIsDone)
            workspace = await CreateWorkspace(workspaceDirectory);

        Console.WriteLine("Ollama service created, getting model list, please wait...");
        var list = await LlmService.GetModels();

        var model = ChooseModel(list);

        Console.WriteLine($"Initialising model '{model.Name}', please wait...");
        await LlmService.InitializeModelAsync(model);

        Console.WriteLine($"Model '{model.Name}' initialized, initialising agents, please wait...");
        var planningAgent = new PlanningAgent(workspace);
        var codingAgent = new CodingAgent(workspace);
        var debuggingAgent = new DebuggingAgent(workspace);
        var projectManagerAgent = new ProjectManagerAgent(workspace);

        Console.WriteLine("Agents initialized, attempting to compile project, please wait...");
        var compileResult = await workspace.Compile();

        Console.WriteLine("Project compile attempt finished, starting lllm-development-cycle, please wait...");
        await MainFlow(workspace, model, planningAgent, codingAgent, debuggingAgent, projectManagerAgent, compileResult);
    }
    private async Task MainFlow(Workspace workspace, OllamaModel model, PlanningAgent planningAgent, CodingAgent codingAgent, DebuggingAgent debuggingAgent, ProjectManagerAgent projectManagerAgent, CompileResult compileResult)
    {
        if (workspace.SubTasks.Count == 0)
        {
            while (!workspace.PlanningIsDone)
            {
                // PLANNING MODE
                compileResult = await AgentFlow(workspace, model, planningAgent, compileResult);
            }
        }

        //// Rerun for debug
        //foreach (var resp in workspace.CodingHistory)
        //    await codingAgent.ProcessResponse(resp.Prompt, resp.Response, false);

        while (!workspace.WorkIsDone)
        {
            while (compileResult.Errors.Count > 0 && workspace.Files.Count > 0)
            {
                // DEBUG MODE
                compileResult = await AgentFlow(workspace, model, debuggingAgent, compileResult);
            }

            // CLEAR DEBUG HISTORY
            workspace.DebugHistory.Clear();
            await workspace.Save();

            while (workspace.WaitingForProjectManagerQuestion != null &&
                workspace.WaitingForProjectManagerToolCallId != null)
            {
                // QUESTION FOR PROJECT MANAGER -MODE
                compileResult = await AgentFlow(workspace, model, projectManagerAgent, compileResult);
            }

            // FEATURE MODE
            compileResult = await AgentFlow(workspace, model, codingAgent, compileResult);
        }

        workspace.WorkIsDone = true;
        await workspace.Save();
    }
    private async Task<CompileResult> AgentFlow(Workspace workspace, OllamaModel model, IAgent agent, CompileResult compileResult)
    {
        workspace.PromptIndex++;
        var hasAnswered = false;
        while (!hasAnswered)
        {
            var prompt = await agent.GeneratePrompt(compileResult);
            Console.WriteLine($"#{workspace.PromptIndex} Asking model:");
            foreach (var message in prompt.messages)
                ShowMessage(message);
            Console.WriteLine();

            Console.WriteLine($"#{workspace.PromptIndex} Model answered:");
            var response = await LlmService.ChatAsync(model, prompt);
            ShowMessage(response.message);
            Console.WriteLine();

            Console.WriteLine($"#{workspace.PromptIndex} Applying answer...");
            hasAnswered = await agent.ProcessResponse(prompt, response);

            await workspace.Save();

            Console.WriteLine($"#{workspace.PromptIndex} Compiling...");
            compileResult = await workspace.Compile();
        }

        return compileResult;
    }

    private async Task<Workspace> CreateWorkspace(string workspaceDirectory)
    {
        var previousColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Please supply a prompt, what do you want to create (use CTRL + enter to submit):");
        string? userPromptText = null;
        var first = true;
        while (userPromptText == null)
        {
            if (first) first = false;
            else
            {
                Console.WriteLine();
                Console.WriteLine("Prompt cannot be empty, please try again:");
            }
            Console.WriteLine();
            userPromptText = ConsoleEditor.ReadMultilineInput();
        }
        var workspace = await Workspace.Create(workspaceDirectory, userPromptText);
        Console.ForegroundColor = previousColor;
        return workspace;
    }
    private OllamaModel ChooseModel(OllamaModel[] list)
    {
        var previousColor = Console.ForegroundColor;
        OllamaModel? model = null;
        while (model == null)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Choose a model:");
            Console.WriteLine();
            for (var i = 0; i < list.Length; i++)
            {
                Console.WriteLine($"{i}. {list[i].Name} (size: {list[i].Size})");
            }
            Console.WriteLine();
            var key = Console.ReadKey();

            if (char.IsDigit(key.KeyChar))
            {
                var choice = key.KeyChar - '0';
                if (choice >= 0 && choice < list.Length)
                {
                    model = list[choice];
                }
            }
        }

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine($"Choosen model: {model.Name}");
        Console.WriteLine();

        Console.ForegroundColor = previousColor;
        return model;
    }
    private void ShowMessage(OllamaMessage message)
    {
        if (!ShownMessages.Add(message)) return;

        var previousColor = Console.ForegroundColor;

        Console.WriteLine($"[{message.role.ToUpper()}]");
        if (message.thinking != null)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(message.thinking);
        }
        if (message.tool_call_id != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message.tool_call_id);
            Console.WriteLine(message.content);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message.content);
        }
        if (message.tool_calls != null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var call in message.tool_calls)
            {
                Console.WriteLine($"{call.id}:");
                Console.WriteLine($"{call.function.name} {JsonSerializer.Serialize(call.function.arguments, DefaultJsonSerializerOptions.JsonSerializeOptions)}");
            }
        }
        Console.WriteLine();

        Console.ForegroundColor = previousColor;
    }

    public void Dispose()
    {
        Cts.Cancel();
        Cts.Dispose();
        LlmService.Dispose();
    }

    // Main entry point for application
    private static async Task Main(string[] args)
    {
        using var program = new Program();
        await program.StartAsync();
    }
}