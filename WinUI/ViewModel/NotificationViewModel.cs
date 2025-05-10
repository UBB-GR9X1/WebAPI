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
        public int _user_id { get; set; }
        public ObservableCollection<Notification> _notifications_collection { get; } = new();

        public NotificationViewModel(NotificationService notification_service, int user_id)
        {
            this._notification_service = notification_service;
            this._user_id = user_id;
        }

        public async Task loadNotificationsAsync(int user_id)
        {
            List<Notification> notifications = await this._notification_service.getNotificationsByUserIdAsync(user_id);

            this._notifications_collection.Clear();

            foreach (Notification notification in notifications)
                this._notifications_collection.Add(notification);
        }

        public async Task deleteNotificationAsync(int notification_id, int user_id)
        {
                await this._notification_service.deleteNotificationAsync(notification_id, user_id);
                Notification notification = this.findNotificationById(notification_id);
                if (notification != null)
                    this._notifications_collection.Remove(notification);
        }

        public Notification findNotificationById(int id)
        {
            foreach (Notification notification in this._notifications_collection)
            {
                if (notification._notification_id == id)
                    return notification;
            }
            return null; 
        }
    }
}