using System.Text;
using FakeItEasy;
using NFluent;
using NUnit.Framework;
using RabbitMQ.Client;

namespace Carrot.Tests
{
    public class BusTests
    {
        private IConnection _connection;
        private IConnectionFactory _connectionFactory;
        private IModel _model;

        [SetUp]
        public void SetUp()
        {
            _connectionFactory = A.Fake<IConnectionFactory>();
            _model = A.Fake<IModel>();
            _connection = A.Fake<IConnection>();

            A.CallTo(() => _connectionFactory.CreateConnection()).Returns(_connection);
            A.CallTo(() => _connection.CreateModel()).Returns(_model);
        }

        [Test]
        public void Should_connect_to_provided_host_when_a_new_bus_is_created()
        {
            var bus = new Bus("host", _connectionFactory);

            Check.That(bus.Host).Equals("host");
        }

        [Test]
        public void Should_create_a_new_connection_when_creating_a_new_bus()
        {
            A.CallTo(() => _connectionFactory.CreateConnection()).Returns(_connection);

            var bus = new Bus("host", _connectionFactory);

            A.CallTo(() => _connectionFactory.CreateConnection()).MustHaveHappened();
        }

        [Test]
        public void Should_ensure_exchange_exists_when_publishing_a_text_message_to_a_specific_exchange()
        {
            var bus = new Bus("host", _connectionFactory);
            bus.Publish("Hello world!", "myExchange");

            A.CallTo(() => _model.ExchangeDeclare("myExchange", A<string>._)).MustHaveHappened();
        }

        [Test]
        public void Should_publish_utf8_message_content_when_a_message_is_published()
        {
            var bytes = Encoding.UTF8.GetBytes("Hello world!");

            var bus = new Bus("host", _connectionFactory);
            bus.Publish("Hello world!", "myExchange");

            A.CallTo(
                () =>
                    _model.BasicPublish(A<string>._, A<string>._, A<IBasicProperties>._,
                        A<byte[]>.That.IsSameSequenceAs(bytes))).MustHaveHappened();
        }

        [Test]
        public void Should_use_provided_exchange_when_publishing_a_message_to_a_specific_exchange()
        {
            var bus = new Bus("host", _connectionFactory);
            bus.Publish("Hello world!", "myExchange");

            A.CallTo(
                () =>
                    _model.BasicPublish("myExchange", A<string>._, A<IBasicProperties>._,
                        A<byte[]>._)).MustHaveHappened();
        }


    }
}