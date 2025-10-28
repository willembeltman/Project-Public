using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace OllamaAgentGenerator.Services;

public class LLMService(string modelName,
    HttpClient? httpClient = null,
    Uri? ollamaServerUrl = null) : IDisposable
{
    private readonly string ModelName = modelName;
    private readonly HttpClient HttpClient = httpClient ?? new();
    private readonly Uri OllamaServerUrl = ollamaServerUrl ?? new Uri("http://localhost:11434");

    public async Task InitializeModelAsync(CancellationToken ct = default)
    {
        var request = new { model = ModelName };
        var url = new Uri(OllamaServerUrl, "/api/pull");
        var response = await HttpClient.PostAsJsonAsync(url, request, ct);
        response.EnsureSuccessStatusCode();
    }

    public record OllamaResponse(
        string? model,
        DateTime? created_at,
        string? thinking,
        string? response,
        bool done);

    public async IAsyncEnumerable<(string? thinking, string? response)> PromptAsync(string prompt, [EnumeratorCancellation] CancellationToken ct = default)
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
            var json = await reader.ReadLineAsync();
            if (json == null)
            {
                break;
            }

            var data = JsonSerializer.Deserialize<OllamaResponse>(json);

            if (data == null || data.done)
            {
                break;
            }

            if (data.thinking != null || data.response != null)
            {
                yield return (data.thinking, data.response);
            }
        }
    }

    public void Dispose()
    {
        HttpClient.Dispose();
    }
}

