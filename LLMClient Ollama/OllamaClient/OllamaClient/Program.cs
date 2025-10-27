using OllamaClientCLI;

internal class Program
{
    private static async Task Main(string[] args)
    {
        using var ollama = new OllamaClient();

        // 1. (Optioneel) Pull het model als het nog niet lokaal staat
        await ollama.PullModelAsync("gpt-oss:20b");

        // 2. Generatie
        string prompt = "Vertel me een kort verhaaltje over een robot die graag koekjes bakt.";
        string result = await ollama.GenerateAsync("gpt-oss:20b", prompt);

        Console.WriteLine("Resultaat:");
        Console.WriteLine(result);
    }
}