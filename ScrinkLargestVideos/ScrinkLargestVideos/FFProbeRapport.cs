using System.Text.Json.Serialization;

namespace ScrinkLargestVideos;

public class FFProbeRapport
{
    [JsonPropertyName("streams")]
    public List<FFProbeStream> Streams { get; set; } = [];
    [JsonPropertyName("format")]
    public FFProbeFormat Format { get; set; } = new();
}
