using Microsoft.Extensions.Configuration;

namespace Rekenkamer.Infrastructure.Data.Extensions;

public static class CreatePdfConfigExtension
{
    public static PdfConfig CreatePdfConfig(this IConfiguration m)
    {
        var config = new PdfConfig();
            //m["OllamaUrl"] ?? "");

        return config;
    }
}
public record PdfConfig();