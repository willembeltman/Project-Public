using System.Text.Json.Serialization;

namespace ScrinkLargestVideos
{
    public class FFProbeTags
    {
        [JsonPropertyName("DURATION")]
        public string Duration { get; set; } = string.Empty;
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        [JsonPropertyName("ENCODER")]
        public string Encoder { get; set; } = string.Empty;
    }
}
