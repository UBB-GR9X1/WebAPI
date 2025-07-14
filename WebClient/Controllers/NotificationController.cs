using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebClient.Services;

namespace WebClient.Controllers
{
    public class NotificationController : Controller
    {
        private readonly NotificationService _notificationService;

        public NotificationController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<IActionResult> Index(int userId)
        {
            var notifications = await _notificationService.GetUserNotificationsAsync(userId);
            return View(notifications);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int notificationId, int userId)
        {
            var success = await _notificationService.DeleteNotificationAsync(notificationId);
            if (success)
            {
                return RedirectToAction(nameof(Index), new { userId });
            }
            return BadRequest("Failed to delete notification");
        }
    }
}