using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pinger.PingHandlers;
using System.Threading.Tasks;

namespace PingerTests.PingHandlersTests
{
	[TestClass]
	public class ICMPPingTest
	{

        [TestMethod]
        public async Task ICMPPing_ValidHost_ReturnTrue()
        {
            // Arrange
            bool expected = true;
            string host = "ya.ru";
            ICMPPing icmpPing = new ICMPPing(host);

            // Act
            bool result = await icmpPing.Ping();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task ICMPPing_ValidHost_ReturnFalse()
        {
            // Arrange
            bool expected = false;
            string host = "my.ru";
            ICMPPing icmpPing = new ICMPPing(host);

            // Act
            bool result = await icmpPing.Ping();

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}
