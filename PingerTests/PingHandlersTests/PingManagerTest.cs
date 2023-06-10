using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pinger.Configuration;
using Pinger.Interfaces;
using Pinger.PingHandlers;
using System;
using System.Collections.Generic;
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
            bool pingResponse = true;

            Mock<ILogger> loggerMock = new Mock<ILogger>();
            IEnumerable<ILogger> loggers = new List<ILogger> { loggerMock.Object };
            
            Mock<AppSettings> appSettings = new Mock<AppSettings>();
            appSettings.Object.host = "ya.ru";
            appSettings.Object.period = 1;

            Mock<IPinger> pingerMock = new Mock<IPinger>();
            pingerMock.Setup(p => p.Ping()).ReturnsAsync(pingResponse);

            PingManager pingManager = new PingManager(loggers, pingerMock.Object, appSettings.Object);

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            // Act
            Task pingManagerTask = Task.Run(() => pingManager.Run(), cancellationToken);

            await Task.Delay(TimeSpan.FromSeconds(2));
            cancellationTokenSource.Cancel();

            // Assert
            loggerMock.Verify(l => l.Log($"{appSettings.Object.host} OK"), Times.AtLeastOnce());

        }

        [TestMethod]
        public async Task PingManager_Run_ValidPing_ResponseLogger_FAILED()
        {
            // Arrange
            bool pingResponse = false;

            Mock<ILogger> loggerMock = new Mock<ILogger>();
            IEnumerable<ILogger> loggers = new List<ILogger> { loggerMock.Object };

            Mock<AppSettings> appSettings = new Mock<AppSettings>();
            appSettings.Object.host = "my.ru";
            appSettings.Object.period = 1;

            Mock<IPinger> pingerMock = new Mock<IPinger>();
            pingerMock.Setup(p => p.Ping()).ReturnsAsync(pingResponse);

            PingManager pingManager = new PingManager(loggers, pingerMock.Object, appSettings.Object);

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            // Act
            Task pingManagerTask = Task.Run(() => pingManager.Run(), cancellationToken);

            await Task.Delay(TimeSpan.FromSeconds(2));
            cancellationTokenSource.Cancel();

            // Assert
            loggerMock.Verify(l => l.Log($"{appSettings.Object.host} FAILED"), Times.AtLeastOnce());

        }
    }
}
