using OllamaAgentGenerator.Agents;
using OllamaAgentGenerator.Services;

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
        if (userPromptText != null)
        {
            var currentDirectoryName = Path.Combine(Environment.CurrentDirectory, "Source");
            var currentDirectory = new DirectoryInfo(currentDirectoryName);
            var fileRepository = new FileRepositoryService(currentDirectory);
            var compiler = new CompilerService(currentDirectory);

            var doPromptAgent = new DoPromptAgent(userPromptText, fileRepository);
            var isPromptFinishedAgent = new IsPromptFinishedAgent(userPromptText, fileRepository);

            Console.WriteLine();

            var i = 0;
            var compileErrors = string.Empty;

            if (fileRepository.HasFiles)
            {
                compileErrors = compiler.Compile();
            }

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;

                // DO WORK
                string fullPromptText = doPromptAgent.GeneratePrompt(compileErrors);
                Console.WriteLine($"###{++i} Ask model:");
                Console.WriteLine();
                Console.WriteLine(fullPromptText);
                Console.WriteLine();

                Console.WriteLine("Model answered:");
                var responseText = await CallLLM(llmService, fullPromptText);
                Console.WriteLine();

                Console.WriteLine("Applying answer to context.");
                Console.WriteLine();
                doPromptAgent.ProcessResponse(responseText);

                Console.ForegroundColor = ConsoleColor.DarkGray;

                // COMPILE
                Console.WriteLine($"###{++i} Compiling...");
                Console.WriteLine();
                compileErrors = compiler.Compile();

                // CHECK
                Console.WriteLine($"###{++i} Ask model:");
                Console.WriteLine();
                fullPromptText = isPromptFinishedAgent.GeneratePrompt(compileErrors);
                Console.WriteLine(fullPromptText);
                Console.WriteLine();

                responseText = string.Empty;
                Console.WriteLine("Model answered:");
                responseText = await CallLLM(llmService, fullPromptText);
                Console.WriteLine();

                Console.WriteLine("Check if prompt has been satisfied.");
                isPromptFinishedAgent.ProcessResponse(responseText);

                if (isPromptFinishedAgent.IsDone) break;
            }
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