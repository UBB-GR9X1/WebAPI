using System;

namespace WebClient.Models
{
    public class Notification
    {
        public int notificationId { get; set; }
        public int userId { get; set; }
        public DateTime deliveryDateTime { get; set; }
        public string message { get; set; }
    }
}