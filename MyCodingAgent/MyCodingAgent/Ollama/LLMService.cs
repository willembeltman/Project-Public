using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyCodingAgent.Ollama;

public class LLMService(
    Uri? ollamaServerUrl = null) : IDisposable
{
    private readonly HttpClient HttpClient = new();
    private readonly Uri OllamaServerUrl = ollamaServerUrl ?? new Uri("http://localhost:11434");

    public async Task<OllamaModel[]> GetModels(CancellationToken ct = default)
    {
        var url = new Uri(OllamaServerUrl, "/api/tags");
        var response = await HttpClient.GetAsync(url, ct);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(ct);
        var data = await JsonSerializer.DeserializeAsync<OllamaTagsResponse>(stream, cancellationToken: ct);

        if (data?.models == null)
            return [];

        return [.. data.models
            .Where(a => !string.IsNullOrWhiteSpace(a.name) && a.size != null && a.modified_at != null)
            .Select(a => new OllamaModel(a.name!, a.size!,a.modified_at!))];
    }

    public async Task InitializeModelAsync(OllamaModel model, CancellationToken ct = default)
    {
        var request = new { model = model.Name };
        var url = new Uri(OllamaServerUrl, "/api/pull");
        var response = await HttpClient.PostAsJsonAsync(url, request, ct);
        response.EnsureSuccessStatusCode();
    }

    public async IAsyncEnumerable<OllamaResponse> PromptAsync(OllamaModel model, string prompt, [EnumeratorCancellation] CancellationToken ct = default)
    {
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        var payload = new
        {
            model = model.Name,
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
                yield break;
            }

            var data = JsonSerializer.Deserialize<OllamaResponseRaw>(json, options);

            if (data == null)
            {
                yield break;
            }

            if (data.done)
            {
                yield break;
            }

            if (data.thinking != null || data.response != null)
            {
                yield return new OllamaResponse(data.thinking, data.response);
            }
        }
    }

    public void Dispose()
    {
        HttpClient.Dispose();
    }
}
