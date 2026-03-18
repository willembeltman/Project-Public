using MyCodingAgent.Agents;
using MyCodingAgent.Helpers;
using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

internal class Program : IDisposable
{
    readonly CancellationTokenSource Cts;
    //readonly HashSet<OllamaMessage> ShownMessages;
    readonly OllamaClient LlmService;

    private Program()
    {
        Cts = new CancellationTokenSource();
        //ShownMessages = new HashSet<OllamaMessage>();
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

        Console.WriteLine("Workspace loaded, getting model list, please wait...");
        var modelList = await LlmService.GetModels();
        var model = ChooseModel(modelList);

        Console.WriteLine($"Initialising model '{model.Name}', please wait...");
        await LlmService.InitializeModelAsync(model);

        Console.WriteLine($"Model '{model.Name}' initialized, initialising agents, please wait...");
        var planningAgent = new ProjectManagerAgent(workspace, LlmService);
        var codingAgent = new CodingAgent(workspace, LlmService);
        var debuggingAgent = new DebugAgent(workspace, LlmService);
        var projectManagerAgent = new ProjectManagerAgentForCodingAgent(workspace, LlmService);

        Console.WriteLine("Agents initialized, attempting to compile workspace, please wait...");
        var compileResult = await workspace.Compile();

        //// Rerun for debug
        //foreach (var resp in workspace.CodingHistory)
        //    await codingAgent.ProcessResponse(resp.Prompt, resp.Response, false);

        //foreach (var resp in workspace.DebugHistory.ToArray())
        //    await debuggingAgent.ProcessResponse(resp.Prompt, resp.Response, false);

        Console.WriteLine("Project compile attempt finished, starting lllm-development-cycle, please wait...");
        await RunMainLoop(workspace, model, planningAgent, codingAgent, debuggingAgent, projectManagerAgent, compileResult);
    }
    private async Task RunMainLoop(
        Workspace workspace,
        OllamaModel model,
        ProjectManagerAgent planningAgent,
        CodingAgent codingAgent,
        DebugAgent debuggingAgent,
        ProjectManagerAgentForCodingAgent projectManagerAgent,
        CompileResult compileResult)
    {
        // -------------------------
        // PLANNING
        // -------------------------
        Console.Clear();
        if (workspace.SubTasks.Count == 0)
        {
            while (!workspace.PlanningIsDone)
            {
                compileResult = await AgentFlow(workspace, model, planningAgent, compileResult);
            }
        }
        Console.Clear();

        // -------------------------
        // MAIN WORK LOOP
        // -------------------------
        while (!workspace.WorkIsDone)
        {
            // DEBUG MODE
            if (NeedsDebugging(workspace, compileResult))
            {
                compileResult = await RunDebugLoop(workspace, model, debuggingAgent, compileResult);
                //ShownMessages.Clear();
                //Console.Clear();
                continue;
            }

            // PROJECT MANAGER MODE
            if (NeedsProjectManager(workspace))
            {
                compileResult = await RunProjectManagerLoop(workspace, model, projectManagerAgent, compileResult);
                //ShownMessages.Clear();
                //Console.Clear();
                continue;
            }

            // FEATURE MODE
            workspace.DebugHistory.Clear();
            await workspace.Save();

            compileResult = await AgentFlow(workspace, model, codingAgent, compileResult);
        }

        await workspace.Save();
    }
    private async Task<CompileResult> RunDebugLoop(Workspace workspace, OllamaModel model, DebugAgent debuggingAgent, CompileResult compileResult)
    {
        while (NeedsDebugging(workspace, compileResult))
        {
            compileResult = await AgentFlow(workspace, model, debuggingAgent, compileResult);
        }

        return compileResult;
    }
    private async Task<CompileResult> RunProjectManagerLoop(Workspace workspace, OllamaModel model, ProjectManagerAgentForCodingAgent projectManagerAgent, CompileResult compileResult)
    {
        while (workspace.CodingAgent_To_ProjectManagerAgent_Question != null)
        {
            compileResult = await AgentFlow(workspace, model, projectManagerAgent, compileResult);
        }

        return compileResult;
    }
    private bool NeedsProjectManager(Workspace workspace)
    {
        return workspace.CodingAgent_To_ProjectManagerAgent_Question != null;
    }
    private bool NeedsDebugging(Workspace workspace, CompileResult compileResult)
    {
        if (workspace.DebugAgent_To_CoderAgent_Question != null ||
            workspace.DebugAgent_To_ProjectManagerAgent_Question != null)
            return false;
        if (workspace.Debugging)
            return true;

        var res = compileResult.Errors.Count > 0 &&
               workspace.Files.Count > 0 &&
               workspace.CodingAgent_To_ProjectManagerAgent_Question == null;
        if (res)
        {
            workspace.Debugging = true;
            //ShownMessages.Clear();
            //Console.Clear();
        }
        return res;
    }

    private async Task<CompileResult> AgentFlow(Workspace workspace, OllamaModel model, IAgent agent, CompileResult compileResult)
    {
        workspace.PromptIndex++;
        var hasToolCalls = false;
        while (!hasToolCalls)
        {
            Console.Clear();
            Console.CursorLeft = 0;
            Console.CursorTop = 0;
            Console.Clear();

            var prompt = await agent.GeneratePrompt(compileResult);
            //Console.WriteLine($"#{workspace.PromptIndex} Asking model:");
            foreach (var message in prompt.messages)
                ShowMessage(message);
            Console.WriteLine();

            //Console.WriteLine($"#{workspace.PromptIndex} Model answered:");
            var response = await LlmService.ChatAsync(model, prompt);
            ShowMessage(response.message);
            Console.WriteLine();

            //Console.WriteLine($"#{workspace.PromptIndex} Applying answer...");
            hasToolCalls = await agent.ProcessResponse(prompt, response);

            await workspace.Save();
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
        //if (!ShownMessages.Add(message)) return;

        var previousColor = Console.ForegroundColor;

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[{message.role.ToUpper()}]");
        if (message.thinking != null)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(message.thinking);
        }
        if (message.tool_call_id != null)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
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
                //Console.WriteLine($"{call.function.name} {JsonSerializer.Serialize(call.function.arguments, DefaultJsonSerializerOptions.JsonSerializeOptions)}");
                Console.WriteLine($"{call.function.name}");

                if (call.function.arguments.action != null)
                    Console.WriteLine($"action: {call.function.arguments.action}");

                if (call.function.arguments.id != null)
                    Console.WriteLine($"id: {call.function.arguments.id}");

                if (call.function.arguments.path != null)
                    Console.WriteLine($"path: {call.function.arguments.path}");

                if (call.function.arguments.query != null)
                    Console.WriteLine($"query: {call.function.arguments.query}");

                if (call.function.arguments.lineNumber != null)
                    Console.WriteLine($"lineNumber: {call.function.arguments.lineNumber}");

                if (call.function.arguments.newPath != null)
                    Console.WriteLine($"newPath: {call.function.arguments.newPath}");

                if (call.function.arguments.content != null)
                    Console.WriteLine($"content: {call.function.arguments.content}");

                if (call.function.arguments.replaceText != null)
                    Console.WriteLine($"replaceText: {call.function.arguments.replaceText}");
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