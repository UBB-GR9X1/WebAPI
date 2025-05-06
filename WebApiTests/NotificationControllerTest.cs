using ClassLibrary.Domain;
using ClassLibrary.IRepository;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace WebApiTests;

[TestClass]
public class NotificationControllerTest
{
    [TestMethod]
    public async Task GetAllNotifications_WithValidController_ReturnsListOfNotification()
    {
        // Arrange
        var _mock_repo = new Mock<INotificationRepository>();
        var _fake_notifications = new List<Notification>
        {
            new Notification { notificationId = 1, userId = 1, message = "Message 1", deliveryDateTime = new DateTime(2025, 3, 2, 12, 32, 0) },
            new Notification { notificationId = 2, userId = 1, message = "Message 2", deliveryDateTime = new DateTime(2025, 5, 1, 23, 59, 30) },
        };
        _mock_repo.Setup(_repo => _repo.getAllNotificationsAsync()).ReturnsAsync(_fake_notifications);

        var _controller = new NotificationController(_mock_repo.Object);

        // Act
        var _result = await _controller.GetAllNotifications();
        var _ok_result = _result.Result as OkObjectResult;

        // Assert
        Assert.IsNotNull(_ok_result);
        var _returned_notifications = _ok_result.Value as List<Notification>;
        Assert.AreEqual(2, _returned_notifications.Count);
    }

    [TestMethod]
    public async Task GetNotificationByUserId_WithValidUserId_ReturnsListOfNotifications()
    {
        // Arrange
        var _valid_notification_id = 1;
        var _mock_repo = new Mock<INotificationRepository>();
        var _fake_notifications = new List<Notification>
        {
            new Notification { notificationId = 1, userId = 1, message = "Message 1", deliveryDateTime = new DateTime(2025, 3, 2, 12, 32, 0) },
            new Notification { notificationId = 2, userId = 1, message = "Message 2", deliveryDateTime = new DateTime(2025, 5, 1, 23, 59, 30) },
        }; 
        _mock_repo.Setup(_repo => _repo.getNotificationsByUserIdAsync(_valid_notification_id)).ReturnsAsync(_fake_notifications.Where(d => d.userId == _valid_notification_id).ToList());

        var _controller = new NotificationController(_mock_repo.Object);

        // Act
        var _result = await _controller.GetNotificationsByUserId(_valid_notification_id);
        var _ok_result = _result.Result as OkObjectResult;

        // Assert
        Assert.IsNotNull(_ok_result);
        var _returned_notifications = _ok_result.Value as List<Notification>;
        Assert.AreEqual(2, _returned_notifications.Count);
    }

    [TestMethod]
    public async Task CreateNotification_WithValidNotification_ReturnsCreatedAtAction()
    {
        // Arrange
        var _mock_repo = new Mock<INotificationRepository>();
        var _fake_notification = new Notification { notificationId = 1, userId = 1, message = "Message 1", deliveryDateTime = new DateTime(2025, 3, 2, 12, 32, 0) };
        _mock_repo.Setup(_repo => _repo.addNotificationAsync(_fake_notification)).Returns(Task.CompletedTask);

        var _controller = new NotificationController(_mock_repo.Object);

        // Act
        var _result = await _controller.CreateNotification(_fake_notification);
        var _created_at = _result as CreatedAtActionResult;

        // Assert
        Assert.IsNotNull(_created_at);
        Assert.AreEqual(_fake_notification.notificationId, ((Notification)_created_at.Value).notificationId);
    }

    [TestMethod]
    public async Task DeleteNotification_WithValidNotificationId_ReturnsNoContent()
    {
        // Arrange
        var _mock_repo = new Mock<INotificationRepository>();
        int _notification_id = 1;

        // No setup needed if the method succeeds (completes without exception)
        _mock_repo.Setup(_repo => _repo.deleteNotificationAsync(_notification_id)).Returns(Task.CompletedTask);

        var _controller = new NotificationController(_mock_repo.Object);

        // Act
        var _result = await _controller.DeleteNotification(_notification_id);

        // Assert
        Assert.IsInstanceOfType(_result, typeof(NoContentResult));
        _mock_repo.Verify(_repo => _repo.deleteNotificationAsync(_notification_id), Times.Once);
    }
}
