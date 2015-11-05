using System;
using Carrot.Model;
using RabbitMQ.Client;

namespace Carrot
{
    public sealed class Bus : IDisposable
    {
        private readonly IConnection _connection;
        private bool _disposed;

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

        public void Publish(string message, Exchange exchange)
        {
            Publish(message, exchange, string.Empty);
        }

        public void Publish(string message, Exchange exchange, IMessageSerializer messageSerializer)
        {
            Publish(message, exchange, string.Empty, messageSerializer);
        }

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

        public void Dispose()
        {
            if (_disposed) return;

            _connection.Dispose();
            _disposed = true;
        }
    }
}