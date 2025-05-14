using Microsoft.AspNetCore.Mvc;
using ClassLibrary.IRepository;
using ClassLibrary.Domain;
using System;

namespace Controllers
{
    [Route("api/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notification_repository;

        public NotificationController(INotificationRepository notification_repository)
        {
            this._notification_repository = notification_repository;
        }

        /// <summary>
        /// Retrieves all notifications.
        /// </summary>
        /// <returns>A list of notifications.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Notification>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Notification>>> getAllNotifications()
        {
            try
            {
                List<Notification> notifications = await this._notification_repository.getAllNotificationsAsync();
                return this.Ok(notifications);
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving notifications: {exception.Message}");
            }
        }

        /// <summary>
        /// Retrieves notifications for a specific user.
        /// </summary>
        /// <param name="user_id">The user's ID.</param>
        /// <returns>List of notifications for the user.</returns>
        [HttpGet("user/{user_id}")]
        [ProducesResponseType(typeof(List<Notification>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Notification>>> getNotificationsByUserId(int user_id)
        {
            try
            {
                List<Notification> notifications = await this._notification_repository.getNotificationsByUserIdAsync(user_id);
                return this.Ok(notifications);
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving user notifications: {exception.Message}");
            }
        }

        /// <summary>
        /// Creates a new notification.
        /// </summary>
        /// <param name="notification">The notification to create.</param>
        /// <returns>Status result.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> createNotification([FromBody] Notification notification)
        {
            if (notification == null)
            {
                return this.BadRequest("Valid notification object is required.");
            }

            try
            {
                await this._notification_repository.addNotificationAsync(notification);
                return this.CreatedAtAction(nameof(getAllNotifications), new { id = notification.notificationId }, notification);
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error creating notification: {exception.Message}");
            }
        }

        /// <summary>
        /// Deletes a notification by its ID.
        /// </summary>
        /// <param name="id">The ID of the notification to delete.</param>
        /// <returns>Status result.</returns>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> deleteNotification(int id)
        {
            try
            {
                await this._notification_repository.deleteNotificationAsync(id);
                return this.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return this.NotFound($"Notification with ID {id} was not found.");
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting notification: {exception.Message}");
            }
        }

        /// <summary>
        /// Retrieves a notification by its ID.
        /// </summary>
        /// <param name="id">The ID of the notification.</param>
        /// <returns>The notification with the specified ID.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Notification), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Notification>> getNotificationById(int id)
        {
            try
            {
                Notification notification = await this._notification_repository.getNotificationByIdAsync(id);
                return this.Ok(notification);
            }
            catch (KeyNotFoundException)
            {
                return this.NotFound($"Notification with ID {id} not found.");
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving notification: {exception.Message}");
            }
        }

    }
}
