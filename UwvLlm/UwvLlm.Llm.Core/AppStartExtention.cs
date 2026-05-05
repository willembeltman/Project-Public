using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UwvLlm.Llm.Core.Services;

namespace UwvLlm.Llm.Core;

public static class AppStartExtention
{
    public static async Task CustomStartAsync(this IHost app)
    {
        using var scope = app.Services.CreateScope();

        var workerService = scope.ServiceProvider.GetRequiredService<ServiceBusReceiverService>();
        var consoleService = scope.ServiceProvider.GetRequiredService<ConsoleService>();

        using var cts = new CancellationTokenSource();

        var workerTask = workerService.Start(cts.Token);
        var consoleTask = consoleService.Start(cts.Token);

        await Task.WhenAny(workerTask, consoleTask);
        cts.Cancel();
        Task.WaitAll(workerTask, consoleTask);
    }
}
