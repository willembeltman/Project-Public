using OllamaAgentGenerator.Agents;
using OllamaAgentGenerator.Services;

Console.WriteLine("Initialising model...");
using var llmService = new LLMService("deepseek-r1:8b");

await llmService.InitializeModelAsync();

Console.WriteLine("Model initialised. Please supply a prompt, what do you want to create:");

var userPromptText = Console.ReadLine();
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

    while (true)
    {
        string fullPromptText = doPromptAgent.GeneratePrompt();
        Console.WriteLine($"Step {++i}: Ask model:"); 
        Console.WriteLine(fullPromptText);
        Console.WriteLine();

        var responseText = string.Empty;
        Console.WriteLine("Model answered:");
        await foreach (var chunk in llmService.PromptAsync(fullPromptText))
        {
            responseText += chunk;
            Console.Write(chunk);
        }
        Console.WriteLine();

        Console.WriteLine("Applying answer to context.");
        Console.WriteLine();
        doPromptAgent.ProcessResponse(responseText);

        if (doPromptAgent.WantsToSeeCompile)
        {
            Console.WriteLine($"Step {++i}: Compiling...");

            var compileErrors = compiler.Compile();
            if (compileErrors != null)
            {
                fullPromptText = doPromptAgent.GeneratePrompt(compileErrors);
                Console.WriteLine($"Step {++i}: Ask model:");
                Console.WriteLine(fullPromptText);
                Console.WriteLine();

                responseText = string.Empty;
                Console.WriteLine("Model answered:");
                await foreach (var chunk in llmService.PromptAsync(fullPromptText))
                {
                    responseText += chunk;
                    Console.Write(chunk);
                }
                Console.WriteLine();

                Console.WriteLine("Applying answer to context.");
                doPromptAgent.ProcessResponse(responseText);
            }
        }

        Console.WriteLine($"Stap {++i}: Checken of de huidige context toereikend is...");

        Console.WriteLine("Volledige vraag aan het model:");
        fullPromptText = isPromptFinishedAgent.GeneratePrompt();
        Console.WriteLine(fullPromptText);
        Console.WriteLine();

        responseText = string.Empty;
        Console.WriteLine("Antwoord van het model:");
        await foreach (var chunk in llmService.PromptAsync(fullPromptText))
        {
            responseText += chunk;
            Console.Write(chunk);
        }
        Console.WriteLine();

        Console.WriteLine("Antwoord (eventueel) uitvoeren op de context");
        isPromptFinishedAgent.ProcessResponse(responseText);

        if (isPromptFinishedAgent.IsDone) break;
    }
}