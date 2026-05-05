using UwvLlm.Api.Core.Enums;

namespace UwvLlm.Infrastructure.Messaging.Interfaces;

public interface IServiceBusReceiver
{
    Task StartAsync(Receipent bus, CancellationToken ct);
}