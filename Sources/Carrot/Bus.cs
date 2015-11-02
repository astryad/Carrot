using System.Text;
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

        public void Publish(string message, string exchangeName, string routingKey)
        {
            var model = _connection.CreateModel();
            model.ExchangeDeclare(exchangeName, "direct");
            model.BasicPublish(exchangeName, routingKey, model.CreateBasicProperties(),
                Encoding.UTF8.GetBytes(message));
        }

        public void Publish(string helloWorld, string exchangeName)
        {
            Publish(helloWorld, exchangeName, string.Empty);
        }
    }
}