namespace ClassLibrary.Domain
{
    /// <summary>
    /// Represents a notification in the system.
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Gets or sets the unique identifier of the notification.
        /// </summary>
        public int _notification_id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user associated with the notification.
        /// </summary>
        public int _user_id { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the notification is scheduled to be delivered.
        /// </summary>
        public DateTime _delivery_date_time { get; set; }

        /// <summary>
        /// Gets or sets the message content of the notification.
        /// </summary>
        public string _message { get; set; }
    }
}
