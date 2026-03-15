using MyCodingAgent.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyCodingAgent.Ollama;

public class OllamaService(
    Uri? ollamaServerUrl = null) : IDisposable
{
    private readonly HttpClient HttpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(3600)
    };
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

    public async Task<OllamaResponse> ChatAsync(
        OllamaModel model, 
        OllamaPrompt prompt, 
        CancellationToken ct = default)
    {
        var payload = $@"{{
  ""model"": ""{model.Name}"",
  ""messages"": {JsonSerializer.Serialize(prompt.messages, Program.JsonSerializeOptions)},
  ""stream"": false,
  ""tools"": [{string.Join(",", prompt.tools.Select(tool => $@"
  {{
    ""type"": ""function"",
    ""function"": {{
      ""name"": ""{tool.Name}"",
      ""description"": ""{tool.Desciption}"",
      ""parameters"": {{
        ""type"": ""object"",
        ""properties"": {{{string.Join(",", tool.Parameters.Select(parameter => $@"
          ""{parameter.Name}"": {{
            ""type"": ""{parameter.Type}"",
            ""description"": ""{parameter.Description}""
          }}"))}
        }},
        ""required"": [{string.Join(",", tool.Parameters.Select(parameter => $@"""{parameter.Name}"""))}]
      }}
    }}
  }}"))}
]
}}";

        var url = new Uri(OllamaServerUrl, "/api/chat");
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(payload)
        };

        var response = await HttpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);
        response.EnsureSuccessStatusCode();

        var agentResponse = 
            await response.Content.ReadFromJsonAsync<OllamaResponse>()
            ?? throw new Exception("Something is not right");

        return agentResponse;
    }

    public void Dispose()
    {
        HttpClient.Dispose();
    }
}
