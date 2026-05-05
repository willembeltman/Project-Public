using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using UwvLlm.Shared.Private.Dtos;
using UwvLlm.Shared.Private.Interfaces;

namespace UwvLlm.Shared.Private.Services;

public class ServiceBusSenderService
{
    private readonly IRabbitConnectionProvider _provider;

    public ServiceBusSenderService(IRabbitConnectionProvider provider)
    {
        _provider = provider;
    }

    public async Task SendAsync<TMessage>(TMessage message, CancellationToken ct)
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
            routingKey: "jobs",
            mandatory: true,
            basicProperties: new BasicProperties { Persistent = true },
            body: body,
            cancellationToken: ct
        );
    }
}