using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUI.Model
{
    internal class Notification
    {
        private int _notification_id;
        private int _user_id;
        private int _doctor_id;
        private string _notification_message;
        private DateTime _notification_date;

        /// <summary>
        /// Initializes a new instance of the <see cref="Notification"/> class with specified values.
        /// </summary>
        /// <param name="notification_id">The unique identifier for the notification.</param>
        /// <param name="user_id">The identifier of the user who receives the notification.</param>
        /// <param name="doctor_id">The identifier of the doctor who sent the notification.</param>
        /// <param name="notification_message">The message content of the notification.</param>
        /// <param name="notification_date">The date and time when the notification was created.</param>
        public Notification(int notification_id, int user_id, int doctor_id, string notification_message, DateTime notification_date)
        {
            this._notification_id = notification_id;
            this._user_id = user_id;
            this._doctor_id = doctor_id;
            this._notification_message = notification_message;
            this._notification_date = notification_date;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Notification"/> class with default values.
        /// </summary>
        public Notification()
        {
            this._notification_id = 0;
            this._user_id = 0;
            this._doctor_id = 0;
            this._notification_message = string.Empty;
            this._notification_date = DateTime.Now;
        }

        /// <summary>
        /// Gets or sets the unique identifier for the notification.
        /// </summary>
        public int Id
        {
            get { return _notification_id; }
            set { _notification_id = value; }
        }

        /// <summary>
        /// Gets or sets the identifier of the user who receives the notification.
        /// </summary>
        public int UserId
        {
            get { return _user_id; }
            set { _user_id = value; }
        }

        /// <summary>
        /// Gets or sets the identifier of the doctor who sent the notification.
        /// </summary>
        public int DoctorId
        {
            get { return _doctor_id; }
            set { _doctor_id = value; }
        }

        /// <summary>
        /// Gets or sets the message content of the notification.
        /// </summary>
        public string NotificationMessage
        {
            get { return _notification_message; }
            set { _notification_message = value; }
        }

        /// <summary>
        /// Gets or sets the date and time when the notification was created.
        /// </summary>
        public DateTime NotificationDate
        {
            get { return _notification_date; }
            set { _notification_date = value; }
        }
    }
}
