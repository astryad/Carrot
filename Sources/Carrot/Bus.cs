using System.Text;
using Carrot.Model;
using RabbitMQ.Client;

namespace Carrot
{
    public class Bus
    {
        private readonly IConnection _connection;

        public Bus(string host)
            : this(host, new ConnectionFactory())
        {
        }

        public Bus(string host, IConnectionFactory connectionFactory)
        {
            var tokens = host.Split('=');
            Host = tokens[1];
            _connection = connectionFactory.CreateConnection();
        }

        public string Host { get; }

        public void Publish(string message, Exchange exchange, string routingKey)
        {
            Publish(message, exchange, routingKey, new Utf8MessageSerializer());
        }

        public void Publish(string message, Exchange exchange, string routingKey, IMessageSerializer messageSerializer)
        {
            using (var model = _connection.CreateModel())
            {
                model.ExchangeDeclare(exchange.Name, exchange.Type);
                var content = messageSerializer.Serialize(message);
                model.BasicPublish(exchange.Name, routingKey, model.CreateBasicProperties(), content);
            }
        }
    }
}