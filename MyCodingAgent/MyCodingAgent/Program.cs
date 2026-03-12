using MyCodingAgent;
using MyCodingAgent.Agents;
using MyCodingAgent.Ollama;

internal class Program
{
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
        if (workspace == null)
            workspace = await CreateWorkspace(workspaceDirectory);

        Console.WriteLine("Workspace loaded. Creating Ollama service, please wait...");
        using var llmService = new LLMService();

        Console.WriteLine("Ollama service created, getting model list, please wait...");
        var list = await llmService.GetModels();

        var model = ChooseModel(list);

        Console.WriteLine($"Initialising model '{model.Name}', please wait...");
        await llmService.InitializeModelAsync(model);

        Console.WriteLine($"Model '{model.Name}' initialized, initialising agents, please wait...");
        var codingAgent = new CodingAgent(workspace);
        var isPromptFinishedAgent = new IsFinishedAgent(workspace);

        Console.WriteLine("Agents initialized, attempting to compile project, please wait...");
        var compileResult = await workspace.CompileAsync();

        Console.WriteLine("Project compile attempt finished, starting lllm-development-cycle, please wait...");
        var index = 0;
        while (true)
        {
            index++;

            // DO WORK
            var hasAnswered = false;
            while (!hasAnswered)
            {
                var fullPromptText = codingAgent.GeneratePrompt(compileResult);
                Console.WriteLine($"#{index} Ask model:");
                Console.WriteLine(fullPromptText);
                Console.WriteLine();

                Console.WriteLine($"#{index} Model answered:");
                var response = await CallLLM(llmService, model, fullPromptText, index);
                Console.WriteLine();

                Console.WriteLine($"#{index} Applying answer...");
                hasAnswered = await codingAgent.ProcessResponse(response);

                await workspace.Save();

                Console.WriteLine($"#{index} Compiling...");
                compileResult = await workspace.CompileAsync();
            }

            // CHECK
            hasAnswered = false;
            while (!hasAnswered)
            {
                var fullPromptText = await isPromptFinishedAgent.GeneratePrompt(compileResult);
                Console.WriteLine($"#{index} Ask model:");
                Console.WriteLine(fullPromptText);
                Console.WriteLine();

                Console.WriteLine($"#{index} Model answered:");
                var response = await CallLLM(llmService, model, fullPromptText, index);
                Console.WriteLine();

                Console.WriteLine($"#{index} Check if prompt has been satisfied.");
                hasAnswered = await isPromptFinishedAgent.ProcessResponse(response);
            }

            if (isPromptFinishedAgent.IsDone) break;
        }
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

        Console.WriteLine($"Choosen model: {model.Name}");
        Console.WriteLine();

        Console.ForegroundColor = previousColor;
        return model;
    }

    private static async Task<Workspace> CreateWorkspace(string workspaceDirectory)
    {
        var previousColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Please supply a prompt, what do you want to create:");
        string? userPromptText = null;
        var first = true;
        while (userPromptText == null)
        {
            if (first) first = false;
            else Console.WriteLine("Prompt cannot be empty, please try again:");
            userPromptText = Console.ReadLine(); // Todo: multiline
        }
        var workspace = await Workspace.Create(workspaceDirectory, userPromptText);
        Console.ForegroundColor = previousColor;
        return workspace;
    }

    private static async Task<AgentResponse> CallLLM(LLMService llmService, OllamaModel model, string fullPromptText, int index)
    {
        var isThinking = false;
        var thinkingText = string.Empty;
        var responseText = string.Empty;
        await foreach (var chunk in llmService.PromptAsync(model, fullPromptText))
        {
            if (!string.IsNullOrEmpty(chunk.thinking))
            {
                if (!isThinking)
                {
                    isThinking = true;
                    Console.WriteLine("Thinking:");
                }
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(chunk.thinking);
                thinkingText += chunk.thinking;
            }
            if (!string.IsNullOrEmpty(chunk.response))
            {
                if (isThinking)
                {
                    isThinking = false;
                    Console.WriteLine("");
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(chunk.response);
                responseText += chunk.response;
            }
        }
        return new (index, responseText, thinkingText);
    }
}
