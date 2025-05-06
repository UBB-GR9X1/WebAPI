using Microsoft.AspNetCore.Mvc;
using ClassLibrary.IRepository;
using ClassLibrary.Domain;

namespace Controllers
{
    [Route("api/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository notificationRepository;

        public NotificationController(INotificationRepository _notificationRepository)
        {
            this.notificationRepository = _notificationRepository;
        }

        /// <summary>
        /// Retrieves all notifications.
        /// </summary>
        /// <returns>A list of notifications.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Notification>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Notification>>> GetAllNotifications()
        {
            try
            {
                List<Notification> notifications = await this.notificationRepository.getAllNotificationsAsync();
                return this.Ok(notifications);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving notifications: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves notifications for a specific user.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <returns>List of notifications for the user.</returns>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(List<Notification>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Notification>>> GetNotificationsByUserId(int userId)
        {
            try
            {
                var notifications = await this.notificationRepository.getNotificationsByUserIdAsync(userId);
                return this.Ok(notifications);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving user notifications: {ex.Message}");
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
        public async Task<ActionResult> CreateNotification([FromBody] Notification notification)
        {
            if (notification == null)
            {
                return this.BadRequest("Valid notification object is required.");
            }

            try
            {
                await this.notificationRepository.addNotificationAsync(notification);
                return this.CreatedAtAction(nameof(GetAllNotifications), new { id = notification.notificationId }, notification);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error creating notification: {ex.Message}");
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
        public async Task<ActionResult> DeleteNotification(int id)
        {
            try
            {
                await this.notificationRepository.deleteNotificationAsync(id);
                return this.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return this.NotFound($"Notification with ID {id} was not found.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting notification: {ex.Message}");
            }
        }
    }
}
