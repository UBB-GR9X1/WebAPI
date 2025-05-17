using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClassLibrary.Domain;
using ClassLibrary.IRepository;
using WinUI.Service;
using Moq;

namespace WinUi_Test.Tests
{
    [TestClass]
    public class NotificationServiceTests
    {
        [TestMethod]
        public async Task getAllNotificationsAsync_returnsListOfNotifications()
        {
            var mockRepo = new Mock<INotificationRepository>();
            mockRepo.Setup(r => r.getAllNotificationsAsync()).ReturnsAsync(new List<Notification>
            {
                new Notification { notificationId = 1, userId = 1, message = "Test", deliveryDateTime = DateTime.Now }
            });

            var service = new NotificationService(mockRepo.Object);
            var result = await service.getAllNotificationsAsync();

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public async Task deleteNotificationAsync_invalidUser_throwsUnauthorizedAccess()
        {
            var mockRepo = new Mock<INotificationRepository>();
            var fakeNotification = new Notification { notificationId = 1, userId = 5 };

            mockRepo.Setup(r => r.getNotificationByIdAsync(1)).ReturnsAsync(fakeNotification);

            var service = new NotificationService(mockRepo.Object);

            await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(
                () => service.deleteNotificationAsync(1, 99));
        }

        [TestMethod]
        public async Task deleteNotificationAsync_notificationNotFound_throwsKeyNotFound()
        {
            var mockRepo = new Mock<INotificationRepository>();
            mockRepo.Setup(r => r.getNotificationByIdAsync(999)).ReturnsAsync((Notification)null);

            var service = new NotificationService(mockRepo.Object);

            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(
                () => service.deleteNotificationAsync(999, 1));
        }

        [TestMethod]
        public async Task deleteNotificationAsync_validUser_deletesNotification()
        {
            var mockRepo = new Mock<INotificationRepository>();
            var notification = new Notification { notificationId = 10, userId = 5 };

            mockRepo.Setup(r => r.getNotificationByIdAsync(10)).ReturnsAsync(notification);
            mockRepo.Setup(r => r.deleteNotificationAsync(10)).Returns(Task.CompletedTask);

            var service = new NotificationService(mockRepo.Object);
            await service.deleteNotificationAsync(10, 5);

            mockRepo.Verify(r => r.deleteNotificationAsync(10), Times.Once);
        }
    }
}

