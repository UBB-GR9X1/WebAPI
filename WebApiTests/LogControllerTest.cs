using ClassLibrary.Domain;
using ClassLibrary.IRepository;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace WebApiTests;

[TestClass]
public class LogControllerTest
{
    [TestMethod]
    public async Task getAllLogs_withValidController_returnsListOfLogs()
    {
        // Arrange
        var _mock_repo = new Mock<ILogRepository>();
        var _fake_logs = new List<Log>
        {
            new Log { log_id = 1, user_id = 1, action_type = "Action 1", timestamp = new DateTime(2025, 3, 2, 12, 32, 0) },
            new Log { log_id = 2, user_id = 1, action_type = "Action 2", timestamp = new DateTime(2025, 5, 1, 23, 59, 30) },
        };
        _mock_repo.Setup(_repo => _repo.getAllLogsAsync()).ReturnsAsync(_fake_logs);

        var _controller = new LogController(_mock_repo.Object);

        // Act
        var _result = await _controller.getAllLogs();
        var _ok_result = _result.Result as OkObjectResult;

        // Assert
        Assert.IsNotNull(_ok_result);
        var _returned_logs = _ok_result.Value as List<Log>;
        Assert.AreEqual(2, _returned_logs.Count);
    }

    [TestMethod]
    public async Task getLogById_withValidLogId_returnsLog()
    {
        // Arrange
        var _mock_repo = new Mock<ILogRepository>();
        var _fake_log = new Log { log_id = 2, user_id = 1, action_type = "Action 2", timestamp = new DateTime(2025, 5, 1, 23, 59, 30) };

        _mock_repo.Setup(_repo => _repo.getLogByIdAsync(1)).ReturnsAsync(_fake_log);

        var _controller = new LogController(_mock_repo.Object);

        // Act
        var _result = await _controller.getLogById(1);
        var _ok_result = _result.Result as OkObjectResult;

        // Assert
        Assert.IsNotNull(_ok_result);
        var _returned_log = _ok_result.Value as Log;
        Assert.AreEqual(2, _returned_log.log_id);
    }

    [TestMethod]
    public async Task createLog_withValidLog_returnsCreatedAtAction()
    {
        // Arrange
        var _mock_repo = new Mock<ILogRepository>();
        var _fake_log = new Log { log_id = 2, user_id = 1, action_type = "Action 2", timestamp = new DateTime(2025, 5, 1, 23, 59, 30) };
        _mock_repo.Setup(_repo => _repo.addLogAsync(_fake_log)).Returns(Task.CompletedTask);

        var _controller = new LogController(_mock_repo.Object);

        // Act
        var _result = await _controller.createLog(_fake_log);
        var _created_at = _result as CreatedAtActionResult;

        // Assert
        Assert.IsNotNull(_created_at);
        Assert.AreEqual(_fake_log.log_id, ((Log)_created_at.Value).log_id);
    }

    [TestMethod]
    public async Task deleteLog_withValidLogId_returnsNoContent()
    {
        // Arrange
        var _mock_repo = new Mock<ILogRepository>();
        int _log_id = 1;

        // No setup needed if the method succeeds (completes without exception)
        _mock_repo.Setup(_repo => _repo.deleteLogAsync(_log_id)).Returns(Task.CompletedTask);

        var _controller = new LogController(_mock_repo.Object);

        // Act
        var _result = await _controller.deleteLog(_log_id);

        // Assert
        Assert.IsInstanceOfType(_result, typeof(NoContentResult));
        _mock_repo.Verify(_repo => _repo.deleteLogAsync(_log_id), Times.Once);
    }
}
