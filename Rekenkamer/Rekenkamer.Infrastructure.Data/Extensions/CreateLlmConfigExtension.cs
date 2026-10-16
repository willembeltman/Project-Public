using Microsoft.Extensions.Configuration;

namespace Rekenkamer.Infrastructure.Data.Extensions;

public static class CreateLlmConfigExtension
{
    public static LlmConfig CreateLlmConfig(this IConfiguration m)
    {
        var config = new LlmConfig(
            m["OllamaUrl"] ?? "http://localhost:11434/",
            m["VectorModel"] ?? "nomic-embed-text",
            Convert.ToInt32(m["VectorDimensions"] ?? "768"));

        return config;
    }
}
public record LlmConfig(string OllamaUrl, string VectorModel, int VectorDimensions);