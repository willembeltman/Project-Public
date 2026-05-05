using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using UwvLlm.Api.Core.Dtos;
using UwvLlm.Api.Core.Enums;
using UwvLlm.Api.Core.Interfaces;

namespace UwvLlm.Api.Core.Services;

public class ServiceBusSender
{
    private readonly IRabbitConnectionProvider _provider;

    public ServiceBusSender(IRabbitConnectionProvider provider)
    {
        _provider = provider;
    }

    public async Task SendAsync<TMessage>(Bus bus, TMessage message, CancellationToken ct)
    {
        var connection = await _provider.GetConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        var envelope = new ServiceBusMessage(
            MessageType: typeof(TMessage).FullName!,
            Payload: JsonSerializer.Serialize(message)
        );

        var json = JsonSerializer.Serialize(envelope);
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(
            exchange: "",
            routingKey: Enum.GetName(bus)!,
            mandatory: true,
            basicProperties: new BasicProperties { Persistent = true },
            body: body,
            cancellationToken: ct
        );
    }
}