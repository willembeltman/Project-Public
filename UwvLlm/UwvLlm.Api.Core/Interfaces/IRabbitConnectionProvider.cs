using RabbitMQ.Client;

namespace UwvLlm.Api.Core.Interfaces;

public interface IRabbitConnectionProvider
{
    Task<IConnection> GetConnectionAsync();
}