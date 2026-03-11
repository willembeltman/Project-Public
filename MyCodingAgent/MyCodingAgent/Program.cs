using MyCodingAgent;
using MyCodingAgent.Agents;
using MyCodingAgent.Services;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Initialising model...");
        //using var llmService = new LLMService("gpt-oss:20b");
        using var llmService = new LLMService("gemma3:4b");

        await llmService.InitializeModelAsync();

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Model initialised. Please supply a prompt, what do you want to create:");

        //var userPromptText = Console.ReadLine();
        var userPromptText = "Create a snake game in C#";
        Console.WriteLine(userPromptText);

        if (userPromptText == null) return;
        await RunTask(llmService, userPromptText);
    }

    private static async Task RunTask(LLMService llmService, string userPromptText)
    {
        var workspaceDirectory = Path.Combine(Environment.CurrentDirectory, "Source");
        var workspace = new Workspace(workspaceDirectory);
        var agent = new Agent(userPromptText, workspace);
        var isPromptFinishedAgent = new IsFinishedAgent(userPromptText, workspace);

        workspace.InitializeAsync();
        Console.WriteLine("Workspace initialized");
        Console.WriteLine();

        var index = 0;
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;

            // DO WORK
            agent.Reset();
            while (!agent.HasAnswered)
            {
                string fullPromptText = agent.GeneratePrompt();
                Console.WriteLine($"###{++index} Ask model:");
                Console.WriteLine();
                Console.WriteLine(fullPromptText);
                Console.WriteLine();

                Console.WriteLine("Model answered:");
                var responseText = await CallLLM(llmService, fullPromptText);
                Console.WriteLine();

                Console.WriteLine("Applying answer.");
                Console.WriteLine();
                agent.ProcessResponse(responseText);
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;

            // COMPILE
            Console.WriteLine($"###{++index} Compiling...");
            Console.WriteLine();
            await workspace.CompileAsync();

            // CHECK
            isPromptFinishedAgent.Reset();
            while (!isPromptFinishedAgent.HasAnswered)
            {
                Console.WriteLine($"###{++index} Ask model:");
                Console.WriteLine();
                var fullPromptText = isPromptFinishedAgent.GeneratePrompt();
                Console.WriteLine(fullPromptText);
                Console.WriteLine();

                var responseText = string.Empty;
                Console.WriteLine("Model answered:");
                responseText = await CallLLM(llmService, fullPromptText);
                Console.WriteLine();

                Console.WriteLine("Check if prompt has been satisfied.");
                isPromptFinishedAgent.ProcessResponse(responseText);
            }

            if (isPromptFinishedAgent.IsDone) break;
        }
    }

    private static async Task<string> CallLLM(LLMService llmService, string fullPromptText)
    {
        var isThinking = false;
        var responseText = string.Empty;
        await foreach (var chunk in llmService.PromptAsync(fullPromptText))
        {
            responseText += chunk.response ?? string.Empty;
            if (!string.IsNullOrEmpty(chunk.thinking))
            {
                if (!isThinking)
                {
                    isThinking = true;
                    Console.WriteLine("Thinking:");
                }
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(chunk.thinking);
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
            }
        }
        return responseText;
    }
}