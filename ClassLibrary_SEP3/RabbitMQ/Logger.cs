using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace ClassLibrary_SEP3.RabbitMQ
{
    public static class Logger
    {
        private const string QueueName = "LogService-Queue";
        private const string ExchangeName = "LogService-Exchange";
        private const string RoutingKey = "foo.bar.baz"; // Adjust routing key as needed

        public static void LogMessage(string message)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = "localhost" // Replace with your RabbitMQ server details
                };

                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: QueueName,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    channel.ExchangeDeclare(ExchangeName, ExchangeType.Topic, durable: true);

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: ExchangeName,
                        routingKey: RoutingKey,
                        basicProperties: null,
                        body: body);

                    Console.WriteLine("Sent message: {0}", message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }
    }
}
