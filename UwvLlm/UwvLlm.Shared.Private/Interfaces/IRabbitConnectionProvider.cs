using RabbitMQ.Client;

namespace UwvLlm.Shared.Private.Interfaces;

public interface IRabbitConnectionProvider
{
    Task<IConnection> GetConnectionAsync();
}