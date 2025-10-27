using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;

public record OllamaResponse(
    string? model, 
    DateTime? created_at, 
    string? response, 
    bool done);

public class LLMService(string ModelName) : IDisposable
{
    private HttpClient HttpClient = new();
    private Uri OllamaServerUrl = new Uri("http://localhost:11434");

    public async Task PullModelAsync(CancellationToken ct = default)
    {
        var request = new { model = ModelName };
        var url = new Uri(OllamaServerUrl, "/api/pull");
        var response = await HttpClient.PostAsJsonAsync(url, request, ct);
        response.EnsureSuccessStatusCode();
    }

    public async IAsyncEnumerable<string> PromptAsync(string prompt, [EnumeratorCancellation] CancellationToken ct = default)
    {
        var payload = new
        {
            model = ModelName,
            prompt = prompt,
            stream = true
        };

        var url = new Uri(OllamaServerUrl, "/api/generate");
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(payload)
        };
        var response = await HttpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);
        response.EnsureSuccessStatusCode();

        var reader = new StreamReader(await response.Content.ReadAsStreamAsync(ct));
        while (!ct.IsCancellationRequested)
        {
            var json = await reader.ReadLineAsync(ct);
            if (string.IsNullOrWhiteSpace(json))
                continue;

            // Response aanmaken
            var data = JsonSerializer.Deserialize<OllamaResponse>(json);

            if (data == null || data.done)
                break;

            if (data.response != null)
                yield return data.response;
        }
    }

    public void Dispose()
    {
        HttpClient.Dispose();
    }
}

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Model aan het initialiseren, wacht even aub");

        using var llmService = new LLMService("gemma3:4b");

        await llmService.PullModelAsync();

        Console.WriteLine("Initialiseren klaar, geef uw opdracht aan de LLM:");

        var prompt = Console.ReadLine();
        if (prompt != null)
        {
            await foreach (var responseChunk in llmService.PromptAsync(prompt))
            {
                Console.Write(responseChunk);
            }
        }
    }
}