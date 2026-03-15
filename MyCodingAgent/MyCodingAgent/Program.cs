using MyCodingAgent.Agents;
using MyCodingAgent.Compile;
using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.Ollama;
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
        if (workspace == null || workspace.UserPromptDone)
            workspace = await CreateWorkspace(workspaceDirectory);

        Console.WriteLine("Workspace loaded. Creating Ollama service, please wait...");
        using var llmService = new OllamaService();

        Console.WriteLine("Ollama service created, getting model list, please wait...");
        var list = await llmService.GetModels();

        var model = ChooseModel(list);

        Console.WriteLine($"Initialising model '{model.Name}', please wait...");
        await llmService.InitializeModelAsync(model);

        Console.WriteLine($"Model '{model.Name}' initialized, initialising agents, please wait...");
        var codingAgent = new CodingAgent(workspace);
        var debuggingAgent = new DebuggingAgent(workspace);
        var isPromptFinishedAgent = new IsFinishedAgent(workspace);

        Console.WriteLine("Agents initialized, attempting to compile project, please wait...");
        var compileResult = await workspace.Compile();

        Console.WriteLine("Project compile attempt finished, starting lllm-development-cycle, please wait...");
        while (true)
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

            // ISDONE CHECK
            var isDone = await IsFinishedFlow(workspace, llmService, model, isPromptFinishedAgent, compileResult);
            if (isDone) break;
        }

        workspace.UserPromptDone = true;
        await workspace.Save();
    }
    private static async Task<CompileResult> ModifyFlow(Workspace workspace, OllamaService llmService, OllamaModel model, IAgent agent, CompileResult compileResult)
    {
        workspace.PromptIndex++;
        var hasAnswered = false;
        while (!hasAnswered)
        {
            var prompt = await agent.GeneratePrompt(compileResult);
            Console.WriteLine($"#{workspace.PromptIndex} Asking model:");
            WritePromptToConsole(prompt);
            Console.WriteLine();

            Console.WriteLine($"#{workspace.PromptIndex} Model answered:");
            var response = await CallLLM(llmService, model, prompt);
            Console.WriteLine();

            Console.WriteLine($"#{workspace.PromptIndex} Applying answer...");
            hasAnswered = await agent.ProcessResponse(prompt, response);

            await workspace.Save();

            //if (workspace.QuestionForDeveloper != null)
            //{
            //    var answer = AskQuestionToDeveloper(workspace.QuestionForDeveloper);
            //    workspace.Tasks.Add(new WorkspaceTask(workspace.QuestionForDeveloper, answer));
            //    await workspace.Save();
            //}

            Console.WriteLine($"#{workspace.PromptIndex} Compiling...");
            compileResult = await workspace.Compile();
        }

        return compileResult;
    }
    private static async Task<bool> IsFinishedFlow(Workspace workspace, OllamaService llmService, OllamaModel model, IsFinishedAgent isPromptFinishedAgent, CompileResult compileResult)
    {
        workspace.PromptIndex++;
        var hasAnswered = false;
        var isDone = false;
        while (!hasAnswered)
        {
            var fullPromptText = await isPromptFinishedAgent.GeneratePrompt(compileResult);
            Console.WriteLine($"#{workspace.PromptIndex} Ask model:");
            WritePromptToConsole(fullPromptText);
            Console.WriteLine();

            Console.WriteLine($"#{workspace.PromptIndex} Model answered:");
            var response = await CallLLM(llmService, model, fullPromptText);
            Console.WriteLine();

            Console.WriteLine($"#{workspace.PromptIndex} Check if prompt has been satisfied.");
            var answer = await isPromptFinishedAgent.ProcessResponse(response);
            hasAnswered = answer.HasValue;
            isDone = answer == true;
        }

        return isDone;
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
    private static void WritePromptToConsole(OllamaPrompt prompt)
    {
        var previousColor = Console.ForegroundColor;

        Console.ForegroundColor = ConsoleColor.White;
        foreach (var message in prompt.messages)
        {
            Console.WriteLine($"[{message.role.ToUpper()}]");
            Console.WriteLine(message.content);
            Console.WriteLine();
        }

        Console.ForegroundColor = previousColor;
    }
    private static async Task<OllamaResponse> CallLLM(OllamaService llmService, OllamaModel model, OllamaPrompt prompt)
    {
        var previousColor = Console.ForegroundColor;

        var response = await llmService.ChatAsync(model, prompt);

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"{response.message.content}");
        Console.WriteLine();
        foreach (var call in response.message.tool_calls)
        {
            Console.WriteLine($"tool_call: {call.function.name}:");
            Console.WriteLine($"{JsonSerializer.Serialize(call.function.arguments, JsonSerializeOptions)}");
        }
        Console.WriteLine();

        Console.ForegroundColor = previousColor;
        return response;
    }
}