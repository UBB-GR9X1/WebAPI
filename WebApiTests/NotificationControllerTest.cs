using ClassLibrary.Domain;
using ClassLibrary.Repository;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace WebApiTests;

[TestClass]
public class NotificationControllerTest
{
    [TestMethod]
    public async Task getAllNotifications_withValidController_returnsListOfNotification()
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
        var _result = await _controller.getAllNotifications();
        var _ok_result = _result.Result as OkObjectResult;

        // Assert
        Assert.IsNotNull(_ok_result);
        var _returned_notifications = _ok_result.Value as List<Notification>;
        Assert.AreEqual(2, _returned_notifications.Count);
    }

    [TestMethod]
    public async Task getNotificationByUserId_withValidUserId_returnsListOfNotifications()
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
        var _result = await _controller.getNotificationsByUserId(_valid_notification_id);
        var _ok_result = _result.Result as OkObjectResult;

        // Assert
        Assert.IsNotNull(_ok_result);
        var _returned_notifications = _ok_result.Value as List<Notification>;
        Assert.AreEqual(2, _returned_notifications.Count);
    }

    [TestMethod]
    public async Task createNotification_withValidNotification_returnsCreatedAtAction()
    {
        // Arrange
        var _mock_repo = new Mock<INotificationRepository>();
        var _fake_notification = new Notification { notificationId = 1, userId = 1, message = "Message 1", deliveryDateTime = new DateTime(2025, 3, 2, 12, 32, 0) };
        _mock_repo.Setup(_repo => _repo.addNotificationAsync(_fake_notification)).Returns(Task.CompletedTask);

        var _controller = new NotificationController(_mock_repo.Object);

        // Act
        var _result = await _controller.createNotification(_fake_notification);
        var _created_at = _result as CreatedAtActionResult;

        // Assert
        Assert.IsNotNull(_created_at);
        Assert.AreEqual(_fake_notification.notificationId, ((Notification)_created_at.Value).notificationId);
    }

    [TestMethod]
    public async Task deleteNotification_withValidNotificationId_returnsNoContent()
    {
        // Arrange
        var _mock_repo = new Mock<INotificationRepository>();
        int _notification_id = 1;

        // No setup needed if the method succeeds (completes without exception)
        _mock_repo.Setup(_repo => _repo.deleteNotificationAsync(_notification_id)).Returns(Task.CompletedTask);

        var _controller = new NotificationController(_mock_repo.Object);

        // Act
        var _result = await _controller.deleteNotification(_notification_id);

        // Assert
        Assert.IsInstanceOfType(_result, typeof(NoContentResult));
        _mock_repo.Verify(_repo => _repo.deleteNotificationAsync(_notification_id), Times.Once);
    }
}
