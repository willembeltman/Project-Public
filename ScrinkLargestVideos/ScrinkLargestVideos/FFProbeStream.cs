using System.Text.Json.Serialization;

namespace ScrinkLargestVideos;

public class FFProbeStream
{
    [JsonPropertyName("index")]
    public int index { get; set; }
    [JsonPropertyName("codec_name")]
    public string codec_name { get; set; } = string.Empty;
    [JsonPropertyName("codec_long_name")]
    public string codec_long_name { get; set; } = string.Empty;
    [JsonPropertyName("profile")]
    public string profile { get; set; } = string.Empty;
    [JsonPropertyName("codec_type")]
    public string codec_type { get; set; } = string.Empty;
    [JsonPropertyName("codec_time_base")]
    public string codec_time_base { get; set; } = string.Empty;
    [JsonPropertyName("codec_tag_string")]
    public string codec_tag_string { get; set; } = string.Empty;
    [JsonPropertyName("codec_tag")]
    public string codec_tag { get; set; } = string.Empty;
    [JsonPropertyName("width")]
    public int? width { get; set; }
    [JsonPropertyName("height")]
    public int? height { get; set; }
    [JsonPropertyName("coded_width")]
    public int? coded_width { get; set; }
    [JsonPropertyName("coded_height")]
    public int? coded_height { get; set; }
    [JsonPropertyName("has_b_frames")]
    public int? has_b_frames { get; set; }
    [JsonPropertyName("sample_aspect_ratio")]
    public string sample_aspect_ratio { get; set; } = string.Empty;
    [JsonPropertyName("display_aspect_ratio")]
    public string display_aspect_ratio { get; set; } = string.Empty;
    [JsonPropertyName("pix_fmt")]
    public string pix_fmt { get; set; } = string.Empty;
    [JsonPropertyName("level")]
    public int? level { get; set; }
    [JsonPropertyName("color_range")]
    public string color_range { get; set; } = string.Empty;
    [JsonPropertyName("color_space")]
    public string color_space { get; set; } = string.Empty;
    [JsonPropertyName("color_transfer")]
    public string color_transfer { get; set; } = string.Empty;
    [JsonPropertyName("color_primaries")]
    public string color_primaries { get; set; } = string.Empty;
    [JsonPropertyName("chroma_location")]
    public string chroma_location { get; set; } = string.Empty;
    [JsonPropertyName("refs")]
    public int? refs { get; set; }
    [JsonPropertyName("r_frame_rate")]
    public string r_frame_rate { get; set; } = string.Empty;
    [JsonPropertyName("avg_frame_rate")]
    public string avg_frame_rate { get; set; } = string.Empty;
    [JsonPropertyName("time_base")]
    public string time_base { get; set; } = string.Empty;
    [JsonPropertyName("start_pts")]
    public int? start_pts { get; set; }
    [JsonPropertyName("start_time")]
    public string start_time { get; set; } = string.Empty;
    [JsonPropertyName("tags")]
    public FFProbeTags tags { get; set; } = new();
    [JsonPropertyName("sample_fmt")]
    public string sample_fmt { get; set; } = string.Empty;
    [JsonPropertyName("sample_rate")]
    public string sample_rate { get; set; } = string.Empty;
    [JsonPropertyName("channels")]
    public int? channels { get; set; }
    [JsonPropertyName("channel_layout")]
    public string channel_layout { get; set; } = string.Empty;
    [JsonPropertyName("bits_per_sample")]
    public int? bits_per_sample { get; set; }
}
