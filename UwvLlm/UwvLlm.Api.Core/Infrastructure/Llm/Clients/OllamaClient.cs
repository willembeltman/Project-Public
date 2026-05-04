using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using UwvLlm.Core.Infrastructure.Llm.Interfaces;
using UwvLlm.Core.Infrastructure.Llm.Models;

namespace UwvLlm.Core.Infrastructure.Llm.Clients;

public class OllamaClient(
    Uri? ollamaServerUrl = null)
    : IDisposable
    , IClient
{
    private readonly HttpClient HttpClient = new() { Timeout = TimeSpan.FromSeconds(3600) };
    private readonly Uri OllamaServerUrl = ollamaServerUrl ?? new Uri("http://localhost:11434");

    public async Task<Model[]> GetModels(CancellationToken ct = default)
    {
        var url = new Uri(OllamaServerUrl, "/api/tags");
        var response = await HttpClient.GetAsync(url, ct);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(ct);
        var data = await JsonSerializer.DeserializeAsync<OllamaModelRawCollection>(stream, cancellationToken: ct);

        if (data?.models == null)
            return [];

        var models = new List<Model>();
        foreach (var model in data.models
            .Where(a =>
                !string.IsNullOrWhiteSpace(a.name) &&
                a.size != null &&
                a.modified_at != null))
        {
            models.Add(new Model(
                model.name!,
                model.size!,
                4096, //8192,
                model.modified_at!));
        }
        return [.. models];
    }

    public async Task InitializeModelAsync(Model model, CancellationToken ct = default)
    {
        var request = new { model = model.Name };
        var url = new Uri(OllamaServerUrl, "/api/pull");
        var response = await HttpClient.PostAsJsonAsync(url, request, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task<LlmResponse> ChatAsync(Model model, LlmRequest apiCall, CancellationToken ct = default)
    {
        string payload = CreateRequestJson(model, apiCall);

        var reponseJson = await DoCall(payload, ct);

        var response =
            JsonSerializer.Deserialize<OllamaResponse>(reponseJson)
            ?? throw new Exception("Something is not right");

        return new LlmResponse(
            response.model,
            response.created_at,
            new Message(
                response.message.role,
                response.message.tool_call_id,
                response.message.content,
                response.message.thinking,
                response.message.tool_calls == null
                ? (ToolCall[]?)null
                :
                [
                    ..response.message.tool_calls.Select(a =>
                        new ToolCall(a.id, new ToolCallFunction(a.function.name, new ToolCallFunctionArguments()
                        {
                            Action = a.function.arguments.action,
                            Content = a.function.arguments.content,
                            Id = a.function.arguments.id,
                            LineNumber = a.function.arguments.lineNumber,
                            NewPath = a.function.arguments.newPath,
                            Path = a.function.arguments.path,
                            Query = a.function.arguments.query
                        })))
                ]));
    }

    private async Task<string> DoCall(string payload, CancellationToken ct)
    {
        var url = new Uri(OllamaServerUrl, "/api/chat");

        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(payload, Encoding.UTF8, "application/json")
        };

        var response = await HttpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(ct);

        return json;
    }

    public string CreateMessagesJson(Message[] messages)
    {
        var ollamaMessages = messages.Select(a =>
            new
            {
                role = a.Role,
                tool_call_id = a.ToolCallId,
                content = a.Content,
                tool_calls = a.ToolCalls?.Select(b =>
                    new
                    {
                        id = b.Id,
                        function = new
                        {
                            name = b.Function.Name,
                            arguments = new
                            {
                                id = b.Function.Arguments.Id,
                                action = b.Function.Arguments.Action,
                                path = b.Function.Arguments.Path,
                                newPath = b.Function.Arguments.NewPath,
                                query = b.Function.Arguments.Query,
                                content = b.Function.Arguments.Content,
                                lineNumber = b.Function.Arguments.LineNumber
                            }
                        }
                    })
            });
        var messagesJson = JsonSerializer.Serialize(ollamaMessages, DefaultJsonSerializerOptions.JsonSerializeOptionsIndented);
        return messagesJson;
    }
    public string CreateToolsJson(Tool[] tools)
    {
        return string.Join(",", tools.Select(tool => $@"
  {{
    ""type"": ""function"",
    ""function"": {{
      ""name"": ""{JsonEscape(tool.Name)}"",
      ""description"": ""{JsonEscape(tool.Desciption)}"",
      ""parameters"": {{
        ""type"": ""object"",
        ""properties"": {{{string.Join(",", tool.Parameters.Select(parameter => $@"
          ""{JsonEscape(parameter.Name)}"": {{
            ""type"": ""{parameter.Type}"",
            ""description"": ""{JsonEscape(parameter.Description)}""{(parameter.Enum == null ? "" : $@",
            ""enum"": [{string.Join(", ", parameter.Enum.Select(e => $@"""{JsonEscape(e)}"""))}]")}
          }}"))}
        }},
        ""required"": [{string.Join(", ", tool.Parameters.Where(p => p.Optional == false).Select(parameter => $@"""{JsonEscape(parameter.Name)}"""))}]
      }}
    }}
  }}"));
    }
    public string CreateRequestJson(Model model, LlmRequest apiCall)
    {
        return $@"{{
  ""model"": ""{model.Name}"",
  ""options"": {{
    ""num_ctx"": 8192
  }},
  ""messages"": {CreateMessagesJson(apiCall.Messages)},
  ""stream"": false,
  ""tools"": [{CreateToolsJson(apiCall.Tools)}]
}}";
    }

    private string? JsonEscape(string? value)
    {
        if (value == null) return null;
        var sb = new System.Text.StringBuilder();
        foreach (var c in value)
        {
            switch (c)
            {
                case '\"': sb.Append("\\\""); break;
                case '\\': sb.Append("\\\\"); break;
                case '\b': sb.Append("\\b"); break;
                case '\f': sb.Append("\\f"); break;
                case '\n': sb.Append("\\n"); break;
                case '\r': sb.Append("\\r"); break;
                case '\t': sb.Append("\\t"); break;
                default:
                    if (char.IsControl(c))
                        sb.Append("\\u" + ((int)c).ToString("x4"));
                    else
                        sb.Append(c);
                    break;
            }
        }
        return sb.ToString();
    }

    public void Dispose()
    {
        HttpClient.Dispose();
        GC.SuppressFinalize(this);
    }
}

internal record OllamaModelRaw(
    string? name,
    long? size,
    string? digest,
    DateTime? modified_at);

internal record OllamaModelRawCollection(
    OllamaModelRaw[]? models);

internal record OllamaResponse(
    string model,
    DateTime created_at,
    OllamaMessage message);
internal record OllamaMessage(
    string role,
    string? tool_call_id,
    string? content,
    string? thinking,
    OllamaToolCall[]? tool_calls);

internal record OllamaToolCall(
    string id,
    OllamaToolCallFunction function);

internal record OllamaToolCallFunction(
    //int? index,
    string name,
    OllamaToolCallFunctionArguments arguments);

internal record OllamaToolCallFunctionArguments(
    string? id,
    string? action,
    string? path,
    string? newPath,
    string? query,
    string? content,
    string? replaceText,
    int? lineNumber);