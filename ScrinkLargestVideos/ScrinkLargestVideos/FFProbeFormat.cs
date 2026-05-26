using System.Text.Json.Serialization;

namespace ScrinkLargestVideos;

public class FFProbeFormat
{
    [JsonPropertyName("filename")]
    public string Filename { get; set; } = string.Empty;
    [JsonPropertyName("nb_streams")]
    public int? StreamCount { get; set; }
    [JsonPropertyName("nb_programs")]
    public int? ProgramCount { get; set; }
    [JsonPropertyName("format_name")]
    public string FormatName { get; set; } = string.Empty;
    [JsonPropertyName("format_long_name")]
    public string FormatNameLong { get; set; } = string.Empty;
    [JsonPropertyName("start_time")]
    public string StartTime { get; set; } = string.Empty;
    [JsonPropertyName("duration")]
    public string Duration { get; set; } = string.Empty;
    [JsonPropertyName("size")]
    public string Size { get; set; } = string.Empty;
    [JsonPropertyName("bit_rate")]
    public string Bitrate { get; set; } = string.Empty;
    [JsonPropertyName("probe_score")]
    public int? ProbeScore { get; set; }
    [JsonPropertyName("tags")]
    public FFProbeTags Tags { get; set; } = new FFProbeTags();
}
