using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Domain;

namespace ClassLibrary.Repository
{
    public interface INotificationRepository
    {
        /// <summary>
        /// Gets all notifications.
        /// </summary>
        /// <returns>A list of notifications</returns>
        Task<List<Notification>> getAllNotificationsAsync();

        /// <summary>
        /// Gets a list of notifications by the user's unique identifier.
        /// </summary>
        /// <param name="id">The id of the user</param>
        /// <returns>The notifications for the given user id.</returns>
        Task<List<Notification>> getNotificationsByUserIdAsync(int user_id);

        /// <summary>
        /// Gets a notification by its id.
        /// </summary>
        /// <param name="id">The id of the notification</param>
        /// <returns>The notifocation with the given id</returns>
        Task<Notification> getNotificationByIdAsync(int id);

        /// <summary>
        /// Adds a new notification to the database.
        /// </summary>
        /// <param name="notification">The notification to be added.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task addNotificationAsync(Notification notification);

        /// <summary>
        /// Deletes a notification by its unique identifier.
        /// </summary>
        /// <param name="id">The id of the notification</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task deleteNotificationAsync(int id);
    }
}
