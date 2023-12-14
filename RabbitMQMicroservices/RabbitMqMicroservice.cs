using System.Text;

namespace RabbitMQMicroservices;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;



public class RabbitMqMicroservice :IDisposable
{
    private readonly ConnectionFactory _factory;
    private readonly IConnection _connection;
    private readonly Dictionary<string, IModel > _channels;

    public RabbitMqMicroservice(string hostName = "localhost")
    {
        _factory = new ConnectionFactory()
        {
            HostName = hostName
        };
        
        _connection = _factory.CreateConnection();
        _channels = new Dictionary<string, IModel>();
    }
    public void DeclareQueue(string queueName)
    {
        if (!_channels.ContainsKey(queueName))
        {
            var channel = _connection.CreateModel();
            _channels[queueName] = channel;
        }

        _channels[queueName].QueueDeclare(queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

    }

    public void SendMessage(string queueName, string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channels[queueName].BasicPublish(exchange: "",
            routingKey: queueName,
            basicProperties:null,
            body: body);
    }

    public void StartListening(string queueName, Action<string> onReceive)
    {
        var consumer = new EventingBasicConsumer(_channels[queueName]);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            onReceive(message);
        };
        _channels[queueName].BasicConsume(queue: queueName,
            autoAck: true,
            consumer: consumer);
    }

    public void Dispose()
    {
        foreach (var channel in _channels.Values)
        {
            channel?.Close();
        }
        _connection?.Close();
    }
}