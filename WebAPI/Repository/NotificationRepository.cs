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
        private readonly ApplicationDbContext dbContext;

        public NotificationRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <inheritdoc/>
        public async Task<List<Notification>> getAllNotificationsAsync()
        {
            var entities = await this.dbContext.Notifications.ToListAsync();

            return entities.Select(entity => new Notification
            {
                notificationId = entity.NotificationId,
                userId = entity.UserId,
                deliveryDateTime = entity.DeliveryDateTime,
                message = entity.Message
            }).ToList();
        }

        /// <inheritdoc/>
        public async Task<List<Notification>> getNotificationsByUserIdAsync(int userId)
        {
            var entities = await this.dbContext.Notifications
                .Where(n => n.UserId == userId)
                .ToListAsync();

            return entities.Select(entity => new Notification
            {
                notificationId = entity.NotificationId,
                userId = entity.UserId,
                deliveryDateTime = entity.DeliveryDateTime,
                message = entity.Message
            }).ToList();
        }

        /// <inheritdoc/>
        public async Task<Notification> getNotificationByIdAsync(int id)
        {
            var entity = await this.dbContext.Notifications.FindAsync(id);

            if (entity == null)
            {
                throw new Exception($"Notification with ID {id} not found.");
            }

            return new Notification
            {
                notificationId = entity.NotificationId,
                userId = entity.UserId,
                deliveryDateTime = entity.DeliveryDateTime,
                message = entity.Message
            };
        }

        /// <inheritdoc/>
        public async Task addNotificationAsync(Notification notification)
        {
            var entity = new NotificationEntity
            {
                UserId = notification.userId,
                DeliveryDateTime = notification.deliveryDateTime,
                Message = notification.message
            };

            this.dbContext.Notifications.Add(entity);
            await this.dbContext.SaveChangesAsync();

            notification.notificationId = entity.NotificationId; // Set the ID after insert
        }

        /// <inheritdoc/>
        public async Task deleteNotificationAsync(int id)
        {
            var entity = await this.dbContext.Notifications.FindAsync(id);

            if (entity == null)
            {
                throw new Exception($"Notification with ID {id} not found.");
            }

            this.dbContext.Notifications.Remove(entity);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
