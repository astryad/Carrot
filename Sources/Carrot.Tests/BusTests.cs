using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using RabbitMQ.Client;

namespace Carrot.Tests
{
    [TestFixture]
    class BusTests
    {
        private IConnectionFactory _connectionFactory;
        private IConnection _connection;

        [SetUp]
        public void Init()
        {
            _connectionFactory = A.Fake<IConnectionFactory>();
            _connection = A.Fake<IConnection>();

            A.CallTo(() => _connectionFactory.CreateConnection()).Returns(_connection);
        }

        [Test]
        public void Should_close_connection_when_disposing_bus()
        {
            using (var bus = new Bus("host=host", _connectionFactory))
            {
                
            }

            A.CallTo(() => _connection.Dispose()).MustHaveHappened();
        }

    }
}
