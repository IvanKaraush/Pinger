using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pinger.Logger;
using System.IO;

namespace PingerTests.LoggerTests
{
    [TestClass]
    public class LoggerTest
    {
        private readonly string filePath = "log.txt";

        [TestMethod]
        public void Log_ValidMessage_LogToFile()
        {
            // Arrange
            string message = "Test message";
            LogToFile logger = new LogToFile();

            // Act
            logger.Log(message);

            // Assert
            Assert.IsTrue(File.Exists(filePath));

            string[] lines = File.ReadAllLines(filePath);
            Assert.IsTrue(lines.Length > 0);

            Dispose();
        }

        public void Dispose()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}