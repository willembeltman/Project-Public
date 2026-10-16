using OllamaSharp;
using Rekenkamer.Infrastructure.Data.Extensions;

namespace Rekenkamer.Backend.Extensions;

public static class TryInitilizeLlmModelsExtension
{
    public static async Task TryInitilizeLlmModels(
        this IServiceProvider servicesProvider,
        LlmConfig llmConfig)
    {
        // Automatisch het Ollama model pullen bij de start van de applicatie
        using (var scope = servicesProvider.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                // Haal de client op uit de DI container
                var ollamaClient = services.GetRequiredService<OllamaApiClient>();

                Console.WriteLine($"[Ollama] Controleren of model '{llmConfig.VectorModel}' aanwezig is...");

                // GECORRIGEERD: We gebruiken nu de await foreach over de IAsyncEnumerable stream
                var statusStream = ollamaClient.PullModelAsync(new OllamaSharp.Models.PullModelRequest
                {
                    Model = llmConfig.VectorModel
                });

                await foreach (var status in statusStream)
                {
                    if (status != null)
                    {
                        Console.WriteLine($"[Ollama] Download status van {llmConfig.VectorModel}: {status.Percent}% - {status.Status}");
                    }
                }

                Console.WriteLine($"[Ollama] Model '{llmConfig.VectorModel}' is succesvol geïnitialiseerd!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Ollama] Fout bij het automatisch binnenhalen van het model: {ex.Message}");
            }
        }
    }
}
