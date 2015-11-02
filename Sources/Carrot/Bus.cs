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
            Host = host;
            _connection = connectionFactory.CreateConnection();
        }

        public string Host { get; }

        public void Publish(string message, string exchangeName)
        {
            var model = _connection.CreateModel();
            model.ExchangeDeclare(exchangeName, "direct");
            model.BasicPublish(exchangeName, string.Empty, model.CreateBasicProperties(),
                Encoding.UTF8.GetBytes(message));
        }
    }
}