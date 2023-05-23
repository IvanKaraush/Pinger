using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pinger.Interfaces;
using Pinger.PingHandlers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PingerTests.PingHandlersTests
{
	[TestClass]
	public class PingManagerTest
	{
        [TestMethod]
        public async Task PingManager_Run_ValidPing_ResponseLogger_OK()
        {
            // Arrange
            string host = "ya.ru";
            double period = 1;
            bool pingResponse = true;

            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IPinger> pingerMock = new Mock<IPinger>();
            pingerMock.Setup(p => p.Ping()).ReturnsAsync(pingResponse);

            PingManager pingManager = new PingManager(loggerMock.Object, pingerMock.Object, host, period);

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            // Act
            Task pingManagerTask = Task.Run(() => pingManager.Run(), cancellationToken);

            await Task.Delay(TimeSpan.FromSeconds(2));
            cancellationTokenSource.Cancel();

            // Assert
            loggerMock.Verify(l => l.Log($"{host} OK"), Times.AtLeastOnce());

        }

        [TestMethod]
        public async Task PingManager_Run_ValidPing_ResponseLogger_FAILED()
        {
            // Arrange
            string host = "ya.ru";
            double period = 1;
            bool pingResponse = false;

            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<IPinger> pingerMock = new Mock<IPinger>();
            pingerMock.Setup(p => p.Ping()).ReturnsAsync(pingResponse);

            PingManager pingManager = new PingManager(loggerMock.Object, pingerMock.Object, host, period);

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            // Act
            Task pingManagerTask = Task.Run(() => pingManager.Run(), cancellationToken);

            await Task.Delay(TimeSpan.FromSeconds(2));
            cancellationTokenSource.Cancel();

            // Assert
            loggerMock.Verify(l => l.Log($"{host} FAILED"), Times.AtLeastOnce());

        }
    }
}
