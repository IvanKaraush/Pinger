using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pinger.PingHandlers;
using System.Threading.Tasks;

namespace PingerTests.PingHandlersTests
{
    [TestClass]
	public class HTTPPingTest
	{
        [TestMethod]
        public async Task HTTPPing_ValidHost_ReturnTrue()
        {
            // Arrange
            bool expected = true;
            string host = "ya.ru";
            int statusCode = 200;
            HTTPPing httpPing = new HTTPPing(host, statusCode);

            // Act
            bool result = await httpPing.Ping();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task HTTPPing_ValidHost_ReturnFalse()
        {
            // Arrange
            bool expected = false;
            string host = "my.ru";
            int statusCode = 200;
            HTTPPing httpPing = new HTTPPing(host, statusCode);

            // Act
            bool result = await httpPing.Ping();

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}
