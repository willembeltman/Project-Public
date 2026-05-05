using UwvLlm.Shared.Private.Dtos;

namespace UwvLlm.Shared.Private.Interfaces;

public interface IHandler
{
}
public interface IHandler<TMessage> : IHandler
{
    Task Handle(TMessage message, CancellationToken ct);
}
