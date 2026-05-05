using UwvLlm.Api.Core.Enums;

namespace UwvLlm.Infrastructure.Messaging.Interfaces;

public interface IServiceBusReceiver
{
    Task Start(Bus bus, CancellationToken ct);
}