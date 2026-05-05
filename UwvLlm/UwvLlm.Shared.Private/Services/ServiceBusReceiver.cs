using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using UwvLlm.Shared.Private.Dtos;
using UwvLlm.Shared.Private.Enums;
using UwvLlm.Shared.Private.Interfaces;

namespace UwvLlm.Shared.Private.Services;

public class ServiceBusReceiver
{
    private readonly IRabbitConnectionProvider _provider;
    private readonly HandlerRegistry _registry;
    private readonly IServiceProvider _sp;

    public ServiceBusReceiver(
        IRabbitConnectionProvider provider,
        HandlerRegistry registry,
        IServiceProvider sp)
    {
        _provider = provider;
        _registry = registry;
        _sp = sp;
    }

    public async Task Start(Bus bus, CancellationToken ct)
    {
        var connection = await _provider.GetConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            Enum.GetName(bus)!,
            durable: true,
            exclusive: false,
            autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (_, e) =>
        {
            var json = Encoding.UTF8.GetString(e.Body.ToArray());

            try
            {
                var message = JsonSerializer.Deserialize<ServiceBusMessage>(json)
                    ?? throw new Exception("Invalid message");

                await _registry.Handle(message, _sp, ct);

                await channel.BasicAckAsync(e.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        };

        await channel.BasicConsumeAsync(Enum.GetName(bus)!, false, consumer);
    }
}