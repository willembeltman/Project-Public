using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UwvLlm.Api.Core.Enums;
using UwvLlm.Infrastructure.Messaging.Interfaces;

namespace UwvLlm.LlmProxy.Extensions;

public static class AppStartExtention
{
    public static async Task StartConsoleAsync(this IHost app)
    {
        using var scope = app.Services.CreateScope();

        var workerService = scope.ServiceProvider.GetRequiredService<IServiceBusReceiver>();
        var consoleService = scope.ServiceProvider.GetRequiredService<IConsoleService>();

        using var cts = new CancellationTokenSource();

        var workerTask = workerService.StartAsync(Receipent.LlmProxy, cts.Token);
        var consoleTask = consoleService.Start(cts.Token);
        await Task.WhenAny(workerTask, consoleTask);

        if (workerTask.Exception != null)
            throw workerTask.Exception;

        cts.Cancel();

        await Task.WhenAll(consoleTask);
    }
}
