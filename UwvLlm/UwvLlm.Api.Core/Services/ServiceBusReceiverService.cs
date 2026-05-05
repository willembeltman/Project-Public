using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using UwvLlm.Api.Core.Infrastructure.Data;

namespace UwvLlm.Api.Core.Services;

public class ServiceBusWorkerService(
    IDbContextFactory<ApplicationDbContext> dbFactory)
    : IDisposable
{
    public async Task Start(CancellationToken ct)
    {
        var factory = new ConnectionFactory() { HostName = "rabbit" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync("jobs", durable: true, exclusive: false, autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (object sender, BasicDeliverEventArgs @event) =>
        {
            var db = await dbFactory.CreateDbContextAsync();
            var body = @event.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            try
            {
                // verwerken
                Console.WriteLine(message);

                await channel.BasicAckAsync(@event.DeliveryTag, multiple: false);
            }
            catch
            {
                // geen ack = retry
            }
        };

        await channel.BasicConsumeAsync(queue: "jobs", autoAck: false, consumer: consumer);
    }
    public void Dispose()
    {
    }
}
