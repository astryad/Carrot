using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using NFluent;
using NUnit.Framework;
using RabbitMQ.Client;

namespace Carrot.Tests
{
    [TestFixture]
    class ConnectionTests
    {
        private Bus _bus;
        private const string ConnectionString = "host=host";

        [SetUp]
        public void Init()
        {
            _bus = new Bus(ConnectionString, A.Fake<IConnectionFactory>());
        }

        [Test]
        public void Should_parse_host_from_connection_string_when_creating_a_new_bus_connection()
        {
            Check.That(_bus.Host).Equals("host");
        }

    }
}
