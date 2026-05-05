using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using UwvLlm.Shared.Private.Dtos;
using UwvLlm.Shared.Private.Interfaces;

namespace UwvLlm.Shared.Private.Services;

public abstract class BaseServiceBusReceiverService
{
    protected abstract IHandler[] Handlers { get; }

    public async Task Start(CancellationToken ct)
    {
        var factory = new ConnectionFactory() { HostName = "rabbit" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync("jobs", durable: true, exclusive: false, autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (object sender, BasicDeliverEventArgs e) =>
        {
            var body = e.Body.ToArray();
            var messageText = Encoding.UTF8.GetString(body);

            try
            {
                // verwerken
                Console.WriteLine(messageText);

                var message = JsonSerializer.Deserialize<ServiceBusMessage>(messageText)
                    ?? throw new Exception("Cannot parse json");

                foreach (var handler in Handlers)
                {
                    if (handler.MessageType == message.MessageType)
                    {
                        await handler.Handle(message.Payload);
                        break;
                    }
                }

                await channel.BasicAckAsync(e.DeliveryTag, multiple: false);
            }
            catch
            {
                // geen ack = retry
            }
        };

        await channel.BasicConsumeAsync(queue: "jobs", autoAck: false, consumer: consumer);
    }
}
