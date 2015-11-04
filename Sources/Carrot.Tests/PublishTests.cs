using Carrot.Model;
using FakeItEasy;
using NUnit.Framework;
using RabbitMQ.Client;

namespace Carrot.Tests
{
    [TestFixture]
    internal class PublishTests
    {
        [SetUp]
        public void Init()
        {
            _connectionFactory = A.Fake<IConnectionFactory>();
            _connection = A.Fake<IConnection>();
            _model = A.Fake<IModel>();

            A.CallTo(() => _connectionFactory.CreateConnection()).Returns(_connection);
            A.CallTo(() => _connection.CreateModel()).Returns(_model);

            _bus = new Bus("host=host", _connectionFactory);
        }

        private IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _model;
        private Bus _bus;

        [Test]
        public void Should_encode_message_content_when_publishing_a_message()
        {
            var messageSerializer = A.Fake<IMessageSerializer>();

            _bus.Publish("Hello world!", new Exchange("exchange", ExchangeType.Direct), "routing.key", messageSerializer);

            A.CallTo(() => messageSerializer.Serialize("Hello world!")).MustHaveHappened();
        }

        [Test]
        public void Should_ensure_specified_exchange_exists_when_publishing_a_message()
        {
            _bus.Publish("Hello world!", new Exchange("exchange", ExchangeType.Direct), "routing.key");

            A.CallTo(() => _model.ExchangeDeclare("exchange", ExchangeType.Direct)).MustHaveHappened();
        }

        [Test]
        public void Should_publish_message_content_using_correct_parameters_when_publishing_a_message()
        {
            _bus.Publish("Hello world!", new Exchange("exchange", ExchangeType.Direct), "routing.key");

            A.CallTo(
                () => _model.BasicPublish("exchange", "routing.key", A<IBasicProperties>.Ignored, A<byte[]>.Ignored))
                .MustHaveHappened();
        }

        [Test]
        public void Should_publish_message_with_no_routing_key_when_no_routing_key_is_provided()
        {
            _bus.Publish("Hello world!", new Exchange("exchange", ExchangeType.Direct));

            A.CallTo(
                () => _model.BasicPublish("exchange", string.Empty, A<IBasicProperties>.Ignored, A<byte[]>.Ignored))
                .MustHaveHappened();
        }

        [Test]
        public void Should_encode_message_content_when_publishing_a_message_with_no_routing_key()
        {
            var messageSerializer = A.Fake<IMessageSerializer>();

            _bus.Publish("Hello world!", new Exchange("exchange", ExchangeType.Direct), messageSerializer);

            A.CallTo(() => messageSerializer.Serialize("Hello world!")).MustHaveHappened();
        }

    }
}