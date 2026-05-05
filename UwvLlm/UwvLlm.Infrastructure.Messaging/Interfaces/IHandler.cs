using UwvLlm.Api.Core.Dtos;

namespace UwvLlm.Api.Core.Interfaces;

public interface IHandler
{
}
public interface IHandler<TMessage> : IHandler
{
    Task Handle(TMessage message, CancellationToken ct);
}
