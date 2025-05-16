using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUI.Model
{
    internal class Notification
    {
        public int notificationId { get; set; }
        public int userId { get; set; }
        public string notificationMessage { get; set; }
        public DateTime notificationDate { get; set; }

        public Notification(int notificationId, int userId, int doctorId, string notificationMessage, DateTime notificationDate)
        {
            this.notificationId = notificationId;
            this.userId = userId;
            this.notificationMessage = notificationMessage;
            this.notificationDate = notificationDate;
        }

        public Notification()
        {
            notificationId = 0;
            userId = 0;
            notificationMessage = string.Empty;
            notificationDate = DateTime.Now;
        }
    }
}