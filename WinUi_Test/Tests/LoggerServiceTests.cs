using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WinUI.Model;
using WinUI.Repository;
using WinUI.Service;

namespace WinUi_Test.Tests
{
    [TestClass]
    public class LoggerServiceTests
    {
        private Mock<ILoggerRepository> _mockRepo;
        private LoggerService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<ILoggerRepository>();
            _service = new LoggerService(_mockRepo.Object);
        }

        [TestMethod]
        public async Task getAllLogs_returnsExpectedList()
        {
            var expected = new List<LogEntryModel>
            {
                new LogEntryModel(1, 1, ActionType.LOGIN, DateTime.UtcNow)
            };
            _mockRepo.Setup(r => r.getAllLogs()).ReturnsAsync(expected);

            var result = await _service.getAllLogs();

            Assert.AreEqual(expected.Count, result.Count);
        }

        [TestMethod]
        public async Task getLogsByUserId_validUserId_returnsLogs()
        {
            var expected = new List<LogEntryModel>
            {
                new LogEntryModel(2, 5, ActionType.CREATE_ACCOUNT, DateTime.UtcNow)
            };
            _mockRepo.Setup(r => r.getLogsByUserId(5)).ReturnsAsync(expected);

            var result = await _service.getLogsByUserId(5);

            Assert.AreEqual(expected.Count, result.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task getLogsByUserId_invalidUserId_throws()
        {
            await _service.getLogsByUserId(0);
        }

        [TestMethod]
        public async Task getLogsByActionType_returnsLogs()
        {
            var expected = new List<LogEntryModel>
            {
                new LogEntryModel(3, 7, ActionType.UPDATE_PROFILE, DateTime.UtcNow)
            };
            _mockRepo.Setup(r => r.getLogsByActionType(ActionType.UPDATE_PROFILE)).ReturnsAsync(expected);

            var result = await _service.getLogsByActionType(ActionType.UPDATE_PROFILE);

            Assert.AreEqual(expected.Count, result.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task getLogsBeforeTimestamp_defaultDate_throws()
        {
            await _service.getLogsBeforeTimestamp(default);
        }

        [TestMethod]
        public async Task getLogsWithParameters_withUserId_callsCorrectMethod()
        {
            var now = DateTime.UtcNow;
            var expected = new List<LogEntryModel>();
            _mockRepo.Setup(r => r.getLogsWithParameters(2, ActionType.DELETE_ACCOUNT, now)).ReturnsAsync(expected);

            var result = await _service.getLogsWithParameters(2, ActionType.DELETE_ACCOUNT, now);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task getLogsWithParameters_withoutUserId_callsAlternateMethod()
        {
            var now = DateTime.UtcNow;
            var expected = new List<LogEntryModel>();
            _mockRepo.Setup(r => r.getLogsWithParametersWithoutUserId(ActionType.DELETE_ACCOUNT, now)).ReturnsAsync(expected);

            var result = await _service.getLogsWithParameters(null, ActionType.DELETE_ACCOUNT, now);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task logAction_validInput_callsRepository()
        {
            _mockRepo.Setup(r => r.logAction(1, ActionType.LOGIN)).ReturnsAsync(true);

            var result = await _service.logAction(1, ActionType.LOGIN);

            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task logAction_invalidUserId_throws()
        {
            await _service.logAction(0, ActionType.LOGIN);
        }
    }
}
