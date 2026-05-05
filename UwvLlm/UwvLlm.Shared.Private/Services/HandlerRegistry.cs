using System.Text.Json;
using UwvLlm.Shared.Private.Dtos;
using UwvLlm.Shared.Private.Interfaces;

namespace UwvLlm.Shared.Private.Services;

public class HandlerRegistry
{
    private readonly Dictionary<string, Func<string, IServiceProvider, CancellationToken, Task>> _handlers;

    public HandlerRegistry(IEnumerable<IHandler> handlers)
    {
        _handlers = new();

        foreach (var handler in handlers)
        {
            var handlerType = handler.GetType();

            var interfaceType = handlerType
                .GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandler<>));

            var messageType = interfaceType.GetGenericArguments()[0];
            var messageTypeName = messageType.FullName!;

            _handlers[messageTypeName] = async (json, sp, ct) =>
            {
                var typedHandler = sp.GetService(handlerType);

                var message = JsonSerializer.Deserialize(json, messageType)
                    ?? throw new Exception("Deserialization failed");

                var method = handlerType.GetMethod("Handle")!;
                var task = (Task)method.Invoke(typedHandler, new[] { message, ct })!;
                await task;
            };
        }
    }

    public Task Handle(ServiceBusMessage message, IServiceProvider sp, CancellationToken ct)
    {
        if (!_handlers.TryGetValue(message.MessageType, out var handler))
            throw new Exception($"No handler for {message.MessageType}");

        return handler(message.Payload, sp, ct);
    }
}