using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFluent;
using NUnit.Framework;

namespace Carrot.Tests
{
    public class BusTests
    {
        [Test]
        public void Should_connect_to_provided_host_when_a_new_bus_is_created()
        {
            var bus = new Bus("host");
            Check.That(bus.Host).Equals("host");
        }
    }
}
