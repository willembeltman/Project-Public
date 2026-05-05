using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using UwvLlm.Api.Core.Dtos;
using UwvLlm.Api.Core.Enums;
using UwvLlm.Infrastructure.Messaging.Interfaces;

namespace UwvLlm.Infrastructure.Messaging.Services;

public class ServiceBusReceiver(
    IRabbitConnectionProvider provider,
    IHandlerRegistry registry,
    IServiceProvider sp,
    IConsoleService console) 
    : IServiceBusReceiver
{
    public async Task StartAsync(Receipent bus, CancellationToken ct)
    {
        var connection = await provider.GetConnectionAsync();
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

                await registry.Handle(message, sp, ct);

                await channel.BasicAckAsync(e.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                console.WriteLine(ex);
            }
        };

        await channel.BasicConsumeAsync(Enum.GetName(bus)!, false, consumer);
    }
}