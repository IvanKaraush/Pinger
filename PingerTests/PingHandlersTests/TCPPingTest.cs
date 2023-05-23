using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pinger.PingHandlers;
using System.Threading.Tasks;

namespace PingerTests.PingHandlersTests
{
	[TestClass]
	public class TCPPingTest
	{
        [TestMethod]
        public async Task TCPPing_ValidHost_ReturnTrue()
        {
            // Arrange
            bool expected = true;
            string host = "ya.ru";
            int port = 80;
            TCPPing icmpPing = new TCPPing(host, port);

            // Act
            bool result = await icmpPing.Ping();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task TCPPing_ValidHost_ReturnFalse()
        {
            // Arrange
            bool expected = false;
            string host = "my.ru";
            int port = 80;
            TCPPing icmpPing = new TCPPing(host, port);

            // Act
            bool result = await icmpPing.Ping();

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}
