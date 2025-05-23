using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Domain;
using ClassLibrary.Repository;
using ClassLibrary;

namespace WinUI.Service

{
    public interface INotificationService
    {
        /// <summary>
        /// Return all Notifications for all users.
        /// </summary>
        Task<List<Notification>> getAllNotificationsAsync();

        /// <summary>
        /// Return all notifications for a specific user.
        /// </summary>
        Task<List<Notification>> getNotificationsByUserIdAsync(int user_id);

        /// <summary>
        /// Delete all notifications with <paramref name="notificationId"/>, 
        /// Only if is from user <paramref name="user_id"/>.
        /// </summary>
        Task deleteNotificationAsync(int notificationId, int user_id);
    }
}
