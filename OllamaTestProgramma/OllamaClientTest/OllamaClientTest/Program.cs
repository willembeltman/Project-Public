using OllamaAgentGenerator.Agents;
using OllamaAgentGenerator.Services;

Console.WriteLine("Model aan het initialiseren...");
using var llmService = new LLMService("gemma3:4b");

await llmService.InitializeModelAsync();

Console.WriteLine("Model geïnitialiseerd. Geef nu uw prompt op:");

var userPromptText = Console.ReadLine();
if (userPromptText != null)
{
    var currentDirectoryName = Path.Combine(Environment.CurrentDirectory, "Source");
    var currentDirectory = new DirectoryInfo(currentDirectoryName);
    var fileRepository = new FileRepositoryService(currentDirectory);
    var compiler = new CompilerService(currentDirectory);
    
    var doPromptAgent = new DoPromptAgent(userPromptText, fileRepository);
    var isPromptFinishedAgent = new IsPromptFinishedAgent(userPromptText, fileRepository);

    Console.WriteLine("Klaar met initialiseren.");
    Console.WriteLine();

    var i = 0;

    while (true)
    {
        Console.WriteLine($"Stap {++i}: Volledige vraag aan het model:"); 
        string fullPromptText = doPromptAgent.GeneratePrompt();
        Console.WriteLine(fullPromptText);
        Console.WriteLine();

        var responseText = string.Empty;
        Console.WriteLine("Antwoord van het model:");
        await foreach (var chunk in llmService.PromptAsync(fullPromptText))
        {
            responseText += chunk;
            Console.Write(chunk);
        }
        Console.WriteLine();

        Console.WriteLine("Antwoord (eventueel) uitvoeren op de context");
        doPromptAgent.ProcessResponse(responseText);

        if (doPromptAgent.WantsToSeeCompile)
        {
            Console.WriteLine($"Stap {++i}: Compileren...");

            var compileErrors = compiler.Compile();
            if (compileErrors != null)
            {
                fullPromptText = doPromptAgent.GeneratePrompt(compileErrors);
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