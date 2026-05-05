using RabbitMQ.Client;
using UwvLlm.Api.Core.Interfaces;

namespace UwvLlm.Api.Core.Services;

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