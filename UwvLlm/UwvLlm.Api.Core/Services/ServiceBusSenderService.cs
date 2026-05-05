using RabbitMQ.Client;
using System.Text;

namespace UwvLlm.Api.Core.Services;

public class ServiceBusSenderService
{
    public async Task SendAsync()
    {
        var factory = new ConnectionFactory() { HostName = "rabbit" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: "jobs",
            durable: true,
            exclusive: false,
            autoDelete: false);

        var body = Encoding.UTF8.GetBytes("hello");

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
