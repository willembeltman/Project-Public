using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UwvLlm.Llm.Core.Services;
using UwvLlm.Shared.Private.Services;

namespace UwvLlm.Llm.Core;

public static class AppStartExtention
{
    public static async Task StartProxyAsync(this IHost app)
    {
        using var scope = app.Services.CreateScope();

        var workerService = scope.ServiceProvider.GetRequiredService<ServiceBusReceiverService>();
        var consoleService = scope.ServiceProvider.GetRequiredService<ConsoleService>();

        using var cts = new CancellationTokenSource();

        var workerTask = workerService.Start(cts.Token);
        var consoleTask = consoleService.Start(cts.Token);

        // Start
        await Task.WhenAny(
            workerTask, 
            consoleTask);

        // Somebody quit, so cancel
        cts.Cancel();

        // Wait for all to quit / throw
        Task.WaitAll(
            workerTask, 
            consoleTask);
    }
}
