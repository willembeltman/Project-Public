using gAPI.Core.Server.Config;
using Microsoft.Extensions.Configuration;

namespace Rekenkamer.ImportApp.Extensions;

public static class CreatePdfConfigExtension
{
    public static PdfConfig CreateLlmConfig(this IConfigurationManager m)
    {
        var config = new PdfConfig(
            m["OllamaUrl"] ?? "");

        return config;
    }
}
public record PdfConfig(string OllamaUrl);