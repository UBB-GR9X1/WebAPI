using ClassLibrary.Domain;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using WinUI.Service;
using System.ComponentModel;

namespace WinUI.ViewModel
{
    public class NotificationViewModel
    {
        private readonly INotificationService _notification_service;
        public int userId { get; set; }
        public ObservableCollection<Notification> notificationsCollection { get; } = new();

        public NotificationViewModel(NotificationService notification_service, int user_id)
        {
            this._notification_service = notification_service;
            this.userId = user_id;
        }

        public async Task loadNotificationsAsync(int user_id)
        {
            List<Notification> notifications = await this._notification_service.getNotificationsByUserIdAsync(user_id);

            this.notificationsCollection.Clear();

            foreach (Notification notification in notifications)
                this.notificationsCollection.Add(notification);
        }

        public async Task deleteNotificationAsync(int notification_id, int user_id)
        {
                await this._notification_service.deleteNotificationAsync(notification_id, user_id);
                Notification notification = this.findNotificationById(notification_id);
                if (notification != null)
                    this.notificationsCollection.Remove(notification);
        }

        public Notification findNotificationById(int id)
        {
            foreach (Notification notification in this.notificationsCollection)
            {
                if (notification.notificationId == id)
                    return notification;
            }
            return null; 
        }
    }
}