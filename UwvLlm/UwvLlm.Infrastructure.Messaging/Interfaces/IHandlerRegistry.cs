using UwvLlm.Api.Core.Dtos;

namespace UwvLlm.Infrastructure.Messaging.Interfaces;

public interface IHandlerRegistry
{
    Task Handle(ServiceBusMessage message, IServiceProvider sp, CancellationToken ct);
}