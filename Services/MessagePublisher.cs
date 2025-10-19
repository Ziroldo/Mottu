using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using Microsoft.Extensions.Configuration;

namespace Mottu.Backend.Services;

public record MotoCreatedEvent(Guid Id, string Identifier, int Year, string Model, string Placa, DateTime CreatedAtUtc);

public class MessagePublisher
{
    private readonly string _host;
    private readonly int _port;
    private readonly string _user;
    private readonly string _password;
    private readonly string _exchange;

    public MessagePublisher(IConfiguration cfg)
    {
        _host     = cfg["RabbitMq:Host"] ?? "localhost";
        _port     = int.TryParse(cfg["RabbitMq:Port"], out var p) ? p : 5672;
        _user     = cfg["RabbitMq:User"] ?? "guest";
        _password = cfg["RabbitMq:Password"] ?? "guest";
        _exchange = cfg["RabbitMq:ExchangeMotoCreated"] ?? "mottu.moto.created";
    }

    private IConnection CreateConnection()
    {
        var factory = new ConnectionFactory
        {
            HostName = _host,
            Port = _port,
            UserName = _user,
            Password = _password,
            DispatchConsumersAsync = true
        };
        return factory.CreateConnection();
    }

    public void PublishMotoCreated(MotoCreatedEvent evt)
    {
        using var conn = CreateConnection();
        using var ch   = conn.CreateModel();

        ch.ExchangeDeclare(exchange: _exchange, type: ExchangeType.Fanout, durable: true, autoDelete: false);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(evt));
        var props = ch.CreateBasicProperties();
        props.ContentType = "application/json";
        props.DeliveryMode = 2;

        ch.BasicPublish(exchange: _exchange, routingKey: string.Empty, basicProperties: props, body: body);
    }
}
