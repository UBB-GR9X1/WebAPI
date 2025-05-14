namespace WebApi.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using ClassLibrary.IRepository;
    using Data;
    using ClassLibrary.Domain;
    using Entity;
    using global::Data;

    /// <summary>
    /// Repository class for managing notification-related database operations.
    /// </summary>
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _db_context;

        public NotificationRepository(ApplicationDbContext db_context)
        {
            this._db_context = db_context;
        }

        /// <inheritdoc/>
        public async Task<List<Notification>> getAllNotificationsAsync()
        {
            List<NotificationEntity> entities = await this._db_context.Notifications.ToListAsync();

            return entities.Select(entity => new Notification
            {
                notificationId = entity.notificationId,
                userId = entity.userId,
                deliveryDateTime = entity.deliveryDateTime,
                message = entity.message
            }).ToList();
        }

        /// <inheritdoc/>
        public async Task<List<Notification>> getNotificationsByUserIdAsync(int user_id)
        {
            List<NotificationEntity> entities = await this._db_context.Notifications
                .Where(notification => notification.userId == user_id)
                .ToListAsync();

            return entities.Select(entity => new Notification
            {
                notificationId = entity.notificationId,
                userId = entity.userId,
                deliveryDateTime = entity.deliveryDateTime,
                message = entity.message
            }).ToList();
        }

        /// <inheritdoc/>
        public async Task<Notification> getNotificationByIdAsync(int id)
        {
            NotificationEntity entity = await this._db_context.Notifications.FindAsync(id);

            if (entity == null)
            {
                throw new Exception($"Notification with ID {id} not found.");
            }

            return new Notification
            {
                notificationId = entity.notificationId,
                userId = entity.userId,
                deliveryDateTime = entity.deliveryDateTime,
                message = entity.message
            };
        }

        /// <inheritdoc/>
        public async Task addNotificationAsync(Notification notification)
        {
            NotificationEntity entity = new NotificationEntity
            {
                userId = notification.userId,
                deliveryDateTime = notification.deliveryDateTime,
                message = notification.message
            };

            this._db_context.Notifications.Add(entity);
            await this._db_context.SaveChangesAsync();

            notification.notificationId = entity.notificationId; // Set the ID after insert
        }

        /// <inheritdoc/>
        public async Task deleteNotificationAsync(int id)
        {
            NotificationEntity entity = await this._db_context.Notifications.FindAsync(id);

            if (entity == null)
            {
                throw new Exception($"Notification with ID {id} not found.");
            }

            this._db_context.Notifications.Remove(entity);
            await this._db_context.SaveChangesAsync();
        }
    }
}
