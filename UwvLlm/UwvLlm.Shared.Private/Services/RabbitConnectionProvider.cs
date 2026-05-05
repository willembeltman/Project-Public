using RabbitMQ.Client;
using UwvLlm.Shared.Private.Interfaces;

namespace UwvLlm.Shared.Private.Services;

public class RabbitConnectionProvider : IRabbitConnectionProvider
{
    private IConnection? _connection;

    public async Task<IConnection> GetConnectionAsync()
    {
        if (_connection != null && _connection.IsOpen)
            return _connection;

        var factory = new ConnectionFactory { HostName = "rabbit" };
        _connection = await factory.CreateConnectionAsync();
        return _connection;
    }
}