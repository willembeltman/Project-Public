using UwvLlm.Shared.Private.Dtos;

namespace UwvLlm.Shared.Private.Interfaces;

public interface IHandler
{
    string MessageType { get; }
    Task Handle(string messageJson);
}
