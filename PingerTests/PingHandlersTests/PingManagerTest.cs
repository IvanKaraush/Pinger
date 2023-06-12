using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pinger.Configuration;
using Pinger.Interfaces;
using Pinger.PingHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
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

            Mock<IUserInteraction> userInteraction = new Mock<IUserInteraction>();
            var loggers = new List<Mock<ILogger>>();

            Mock<AppSettings> appSettings = new Mock<AppSettings>();
            appSettings.Object.host = "ya.ru";
            appSettings.Object.period = 1;

            Mock<IPinger> pingerMock = new Mock<IPinger>();
            pingerMock.Setup(p => p.Ping()).ReturnsAsync(pingResponse);

            PingManager pingManager = new PingManager(userInteraction.Object, loggers.Select(l => l.Object), pingerMock.Object, appSettings.Object);

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            // Act
            Task pingManagerTask = Task.Run(() => pingManager.Run(), cancellationToken);

            await Task.Delay(TimeSpan.FromSeconds(2));
            cancellationTokenSource.Cancel();

            // Assert
            foreach (var item in loggers)
            {
                item.Verify(l => l.Log($"{appSettings.Object.host} FAILED"), Times.AtLeastOnce());
            }
        }

        [TestMethod]
        public async Task PingManager_Run_ValidPing_ResponseLogger_FAILED()
        {
            // Arrange
            bool pingResponse = false;

            Mock<IUserInteraction> userInteraction = new Mock<IUserInteraction>();
            var loggers = new List<Mock<ILogger>>();

            Mock<AppSettings> appSettings = new Mock<AppSettings>();
            appSettings.Object.host = "my.ru";
            appSettings.Object.period = 1;

            Mock<IPinger> pingerMock = new Mock<IPinger>();
            pingerMock.Setup(p => p.Ping()).ReturnsAsync(pingResponse);

            PingManager pingManager = new PingManager(userInteraction.Object, loggers.Select(l => l.Object), pingerMock.Object, appSettings.Object);

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            // Act
            Task pingManagerTask = Task.Run(() => pingManager.Run(), cancellationToken);

            await Task.Delay(TimeSpan.FromSeconds(2));
            cancellationTokenSource.Cancel();

			// Assert
			foreach (var item in loggers)
			{
                item.Verify(l => l.Log($"{appSettings.Object.host} FAILED"), Times.AtLeastOnce());
            }

        }
    }
}
