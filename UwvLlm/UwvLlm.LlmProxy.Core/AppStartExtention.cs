using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UwvLlm.Api.Core.Enums;
using UwvLlm.Infrastructure.Messaging.Interfaces;
using UwvLlm.LlmProxy.Core.Services;

namespace UwvLlm.LlmProxy.Core;

public static class AppStartExtention
{
    public static async Task StartConsoleAsync(this IHost app)
    {
        using var scope = app.Services.CreateScope();

        var workerService = scope.ServiceProvider.GetRequiredService<IServiceBusReceiver>();
        var consoleService = scope.ServiceProvider.GetRequiredService<ConsoleService>();

        using var cts = new CancellationTokenSource();

        await workerService.Start(Bus.LlmProxy, cts.Token);
    }
}
