using MyCodingAgent.Agents;
using MyCodingAgent.Helpers;
using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

internal class Program
{
    public static JsonSerializerOptions JsonSerializeOptions = new JsonSerializerOptions
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true,
    }; 
    public static JsonSerializerOptions JsonSerializeOptionsNotIndented = new JsonSerializerOptions
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
    public static JsonSerializerOptions JsonDeserializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };
    static HashSet<OllamaMessage> shownMessages = new HashSet<OllamaMessage>();

    private static async Task Main(string[] args)
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

        Console.WriteLine("Workspace loaded. Creating Ollama service, please wait...");
        using var llmService = new OllamaClient();

        Console.WriteLine("Ollama service created, getting model list, please wait...");
        var list = await llmService.GetModels();

        var model = ChooseModel(list);

        Console.WriteLine($"Initialising model '{model.Name}', please wait...");
        await llmService.InitializeModelAsync(model);

        Console.WriteLine($"Model '{model.Name}' initialized, initialising agents, please wait...");
        var planningAgent = new PlanningAgent(workspace);
        var codingAgent = new CodingAgent(workspace);
        var debuggingAgent = new DebuggingAgent(workspace);

        Console.WriteLine("Agents initialized, attempting to compile project, please wait...");
        var compileResult = await workspace.Compile();

        Console.WriteLine("Project compile attempt finished, starting lllm-development-cycle, please wait...");

        if (workspace.SubTasks.Count == 0)
        {
            while (!workspace.PlanningIsDone)
            {
                // PLANNING MODE
                compileResult = await ModifyFlow(workspace, llmService, model, planningAgent, compileResult);
            }
        }

        while (!workspace.WorkIsDone)
        {
            while (compileResult.Errors.Count > 0 && workspace.Files.Count > 0)
            {
                // DEBUG MODE
                compileResult = await ModifyFlow(workspace, llmService, model, debuggingAgent, compileResult);
            }

            // CLEAR DEBUG HISTORY
            workspace.DebugHistory.Clear();
            await workspace.Save();

            // FEATURE MODE
            compileResult = await ModifyFlow(workspace, llmService, model, codingAgent, compileResult);
        }

        workspace.WorkIsDone = true;
        await workspace.Save();
    }
    private static async Task<CompileResult> ModifyFlow(Workspace workspace, OllamaClient llmService, OllamaModel model, IAgent agent, CompileResult compileResult)
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
            var response = await llmService.ChatAsync(model, prompt);
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
   
    private static async Task<Workspace> CreateWorkspace(string workspaceDirectory)
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
    private static OllamaModel ChooseModel(OllamaModel[] list)
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

    private static void ShowMessage(OllamaMessage message)
    {
        if (!shownMessages.Add(message)) return;

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
                Console.WriteLine($"{call.function.name} {JsonSerializer.Serialize(call.function.arguments, JsonSerializeOptionsNotIndented)}");
            }
        }
        Console.WriteLine();

        Console.ForegroundColor = previousColor;
    }
}