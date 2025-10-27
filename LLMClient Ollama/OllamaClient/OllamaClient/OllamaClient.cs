using System.Net.Http.Json;

namespace OllamaClientCLI;

public class OllamaClient : IDisposable
{
	private readonly HttpClient _http;
	private readonly Uri _baseUri;

	public OllamaClient(string baseAddress = "http://localhost:11434")
	{
		_http = new HttpClient();
		_baseUri = new Uri(baseAddress);
	}

	// ---------- Pull a model ----------
	public async Task PullModelAsync(string modelName, CancellationToken ct = default)
	{
		var req = new { model = modelName };
		var resp = await _http.PostAsJsonAsync(new Uri(_baseUri, "/api/pull"), req, ct);
		resp.EnsureSuccessStatusCode();
		// Response is a stream of progress messages; you can stream it if desired.
	}

	// ---------- Generate text ----------
	public async Task<string> GenerateAsync(
		string model,
		string prompt,
		bool stream = false,
		CancellationToken ct = default)
	{
		var payload = new
		{
			model,
			prompt,
			stream // bool; if true, you receive a server‑sent event stream
		};

		if (!stream)
		{
			// Simple request – single response
			var response = await _http.PostAsJsonAsync(
				new Uri(_baseUri, "/api/generate"),
				payload,
				ct);

			response.EnsureSuccessStatusCode();

			// Response body contains {"response":"…","model":"…"}
			var json = await response.Content.ReadFromJsonAsync<GenerateResponse>(ct);
			return json?.Response ?? string.Empty;
		}
		else
		{
			// Streaming – return the first chunk (or process all chunks)
			var request = new HttpRequestMessage(HttpMethod.Post, new Uri(_baseUri, "/api/generate"))
			{
				Content = JsonContent.Create(payload)
			};

			var resp = await _http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);
			resp.EnsureSuccessStatusCode();

			// The server sends a chunked text/event-stream; we read it line‑by‑line.
			var streamReader = new System.IO.StreamReader(await resp.Content.ReadAsStreamAsync(ct));
			string? line;
			string fullResponse = string.Empty;
			while ((line = await streamReader.ReadLineAsync()) != null && !ct.IsCancellationRequested)
			{
				if (line.StartsWith("data: "))
					fullResponse += line[6..]; // Append the data part
			}

			return fullResponse;
		}
	}

	public void Dispose() => _http.Dispose();

	// ------- Response DTO -------
	private class GenerateResponse
	{
		public string? Response { get; set; }
		public string? Model { get; set; }
	}
}
