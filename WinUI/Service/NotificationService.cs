using ClassLibrary.IRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClassLibrary.Domain;

namespace WinUI.Service
{
    /// <summary>
    /// Provides high-level notification operations on top of the _notification_repository.
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notification_repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationService"/> class.
        /// </summary>
        /// <param name="notification_repository">The notification repository used for data access.</param>
        public NotificationService(INotificationRepository notification_repository)
        {
            this._notification_repository = notification_repository;
        }

        /// <summary>
        /// Retrieves all notifications across all users.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation, containing the list of all notifications.
        /// </returns>
        public Task<List<Notification>> getAllNotificationsAsync()
        {
            return this._notification_repository.getAllNotificationsAsync();
        }

        /// <summary>
        /// Retrieves all notifications for a specific user.
        /// </summary>
        /// <param name="user_id">The unique identifier of the user.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing the list of notifications for the given user.
        /// </returns>
        public Task<List<Notification>> getNotificationsByUserIdAsync(int user_id)
        {
            return this._notification_repository.getNotificationsByUserIdAsync(user_id);
        }

        /// <summary>
        /// Deletes a single notification if it belongs to the specified user.
        /// </summary>
        /// <param name="notification_id">The unique identifier of the notification.</param>
        /// <param name="user_id">The unique identifier of the user attempting the deletion.</param>
        /// <returns>
        /// A task representing the asynchronous delete operation.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when no notification with the specified ID exists.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// Thrown when the notification does not belong to the given user.
        /// </exception>
        public async Task deleteNotificationAsync(int notification_id, int user_id)
        {
            Notification _notification = await this._notification_repository.getNotificationByIdAsync(notification_id);

            if (_notification == null)
                throw new KeyNotFoundException(
                    $"Notification with ID {notification_id} not found.");

            if (_notification._user_id != user_id)
                throw new UnauthorizedAccessException(
                    $"User {user_id} is not allowed to delete notification {notification_id}.");

            await this._notification_repository.deleteNotificationAsync(notification_id);
        }
    }
}
