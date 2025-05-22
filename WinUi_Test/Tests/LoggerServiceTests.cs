using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClassLibrary.Domain;
using ClassLibrary.Repository;
using WinUI.Model;
using WinUI.Service;

namespace WinUi_Test
{
    [TestClass]
    public class LoggerServiceTests
    {
        private Mock<ILogRepository> mockRepo;
        private LoggerService loggerService;

        [TestInitialize]
        public void Setup()
        {
            mockRepo = new Mock<ILogRepository>();
            loggerService = new LoggerService(mockRepo.Object);
        }

        [TestMethod]
        [TestCategory("LoggerService")]
        [Description("Should return all logs converted to LogEntryModels")]
        public async Task GetAllLogs_ReturnsAllLogs()
        {
            // Arrange
            var logs = new List<Log> { new Log { logId = 1, userId = 10, actionType = "LOGIN", timestamp = DateTime.UtcNow } };
            mockRepo.Setup(r => r.getAllLogsAsync()).ReturnsAsync(logs);

            // Act
            var result = await loggerService.getAllLogs();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(10, result[0].user_id);
        }

        [TestMethod]
        [TestCategory("LoggerService")]
        [Description("Should filter logs by valid user ID")]
        public async Task GetLogsByUserId_ValidId_ReturnsFilteredLogs()
        {
            var logs = new List<Log>
            {
                new Log { logId = 1, userId = 1, actionType = "LOGIN", timestamp = DateTime.UtcNow },
                new Log { logId = 2, userId = 2, actionType = "LOGOUT", timestamp = DateTime.UtcNow }
            };
            mockRepo.Setup(r => r.getAllLogsAsync()).ReturnsAsync(logs);

            var result = await loggerService.getLogsByUserId(1);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result[0].user_id);
        }

        [TestMethod]
        [TestCategory("LoggerService")]
        [Description("Should throw exception for invalid user ID")]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetLogsByUserId_InvalidId_ThrowsException()
        {
            await loggerService.getLogsByUserId(0);
        }

        [TestMethod]
        [TestCategory("LoggerService")]
        [Description("Should filter logs by action type")]
        public async Task GetLogsByActionType_ReturnsFilteredLogs()
        {
            var logs = new List<Log>
            {
                new Log { logId = 1, userId = 1, actionType = "LOGIN", timestamp = DateTime.UtcNow },
                new Log { logId = 2, userId = 2, actionType = "LOGOUT", timestamp = DateTime.UtcNow }
            };
            mockRepo.Setup(r => r.getAllLogsAsync()).ReturnsAsync(logs);

            var result = await loggerService.getLogsByActionType(ActionType.LOGIN);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(ActionType.LOGIN, result[0].action_type);
        }

        [TestMethod]
        [TestCategory("LoggerService")]
        [Description("Should filter logs before given timestamp")]
        public async Task GetLogsBeforeTimestamp_ReturnsFilteredLogs()
        {
            var timestamp = DateTime.UtcNow;
            var logs = new List<Log>
            {
                new Log { logId = 1, userId = 1, actionType = "LOGIN", timestamp = timestamp.AddMinutes(-10) },
                new Log { logId = 2, userId = 1, actionType = "LOGIN", timestamp = timestamp.AddMinutes(10) }
            };
            mockRepo.Setup(r => r.getAllLogsAsync()).ReturnsAsync(logs);

            var result = await loggerService.getLogsBeforeTimestamp(timestamp);

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        [TestCategory("LoggerService")]
        [Description("Should throw exception for default timestamp")]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetLogsBeforeTimestamp_Default_ThrowsException()
        {
            await loggerService.getLogsBeforeTimestamp(default);
        }

        [TestMethod]
        [TestCategory("LoggerService")]
        [Description("Should filter logs by user, action type and timestamp")]
        public async Task GetLogsWithParameters_ReturnsFilteredLogs()
        {
            var timestamp = DateTime.UtcNow;
            var logs = new List<Log>
            {
                new Log { logId = 1, userId = 5, actionType = "LOGIN", timestamp = timestamp.AddMinutes(-5) },
                new Log { logId = 2, userId = 6, actionType = "LOGIN", timestamp = timestamp.AddMinutes(-10) }
            };
            mockRepo.Setup(r => r.getAllLogsAsync()).ReturnsAsync(logs);

            var result = await loggerService.getLogsWithParameters(5, ActionType.LOGIN, timestamp);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(5, result[0].user_id);
        }

        [TestMethod]
        [TestCategory("LoggerService")]
        [Description("Should log action and return true on success")]
        public async Task LogAction_ValidInput_ReturnsTrue()
        {
            mockRepo.Setup(r => r.addLogAsync(It.IsAny<Log>())).Returns(Task.CompletedTask);

            var result = await loggerService.logAction(1, ActionType.LOGIN);

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("LoggerService")]
        [Description("Should return false if exception occurs when logging action")]
        public async Task LogAction_RepositoryThrowsException_ReturnsFalse()
        {
            mockRepo.Setup(r => r.addLogAsync(It.IsAny<Log>())).Throws(new Exception());

            var result = await loggerService.logAction(1, ActionType.LOGIN);

            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("LoggerService")]
        [Description("Should throw exception for invalid user ID in logAction")]
        [ExpectedException(typeof(ArgumentException))]
        public async Task LogAction_InvalidUserId_ThrowsException()
        {
            await loggerService.logAction(0, ActionType.LOGIN);
        }
    }
}
