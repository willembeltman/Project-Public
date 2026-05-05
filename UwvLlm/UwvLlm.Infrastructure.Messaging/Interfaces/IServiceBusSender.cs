using UwvLlm.Api.Core.Enums;

namespace UwvLlm.Infrastructure.Messaging.Interfaces;

public interface IServiceBusSender
{
    Task SendAsync<TMessage>(Bus bus, TMessage message, CancellationToken ct);
}