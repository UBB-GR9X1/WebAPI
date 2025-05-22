using ClassLibrary.Service;
using ClassLibrary.Domain;
using ClassLibrary.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebClient.Models;

namespace WebClient.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILoggerService _loggerService;
        private readonly IAuthService _authService;

        public AdminController(ILoggerService loggerService, IAuthService authService)
        {
            _loggerService = loggerService;
            _authService = authService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var model = new LogFilterViewModel
            {
                logs = await _loggerService.getAllLogs(),
                selected_date = DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> FilterLogs(LogFilterViewModel filter)
        {
            var logs = new List<LogEntryModel>();

            if (!string.IsNullOrEmpty(filter.user_id) && int.TryParse(filter.user_id, out int userId) &&
                filter.selected_action_type.HasValue && filter.selected_date.HasValue)
            {
                logs = await _loggerService.getLogsWithParameters(userId, filter.selected_action_type.Value, filter.selected_date.Value);
            }
            else if (!string.IsNullOrEmpty(filter.user_id) && int.TryParse(filter.user_id, out userId))
            {
                logs = await _loggerService.getLogsByUserId(userId);
            }
            else if (filter.selected_action_type.HasValue)
            {
                logs = await _loggerService.getLogsByActionType(filter.selected_action_type.Value);
            }
            else if (filter.selected_date.HasValue)
            {
                logs = await _loggerService.getLogsBeforeTimestamp(filter.selected_date.Value);
            }
            else
            {
                logs = await _loggerService.getAllLogs();
            }

            filter.logs = logs;
            return View("Dashboard", filter);
        }

        public async Task<IActionResult> LogDetails(int id)
        {
            LogEntryModel log = await _loggerService.getLogById(id);
            if (log == null)
                return NotFound();

            return View(log);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLog(int id)
        {
            await _loggerService.deleteLog(id);
            return RedirectToAction(nameof(Dashboard));
        }

        [HttpGet]
        public async Task<IActionResult> AllLogs()
        {
            var logs = await _loggerService.getAllLogs();
            return PartialView("_LogsTable", logs);
        }

        [HttpGet]
        public async Task<IActionResult> FilterByUserId(string userId)
        {
            if (int.TryParse(userId, out int id))
            {
                var logs = await _loggerService.getLogsByUserId(id);
                return PartialView("_LogsTable", logs);
            }
            return BadRequest("Invalid User ID");
        }

        [HttpGet]
        public async Task<IActionResult> FilterByActionType(ActionType actionType)
        {
            var logs = await _loggerService.getLogsByActionType(actionType);
            return PartialView("_LogsTable", logs);
        }

        [HttpGet]
        public async Task<IActionResult> FilterByTimestamp(DateTime timestamp)
        {
            var logs = await _loggerService.getLogsBeforeTimestamp(timestamp);
            return PartialView("_LogsTable", logs);
        }

        [HttpGet]
        public async Task<IActionResult> ApplyFilters(string userId, ActionType? actionType, DateTime? timestamp)
        {
            int? parsedUserId = null;
            if (!string.IsNullOrEmpty(userId) && int.TryParse(userId, out int id))
            {
                parsedUserId = id;
            }

            var logs = await _loggerService.getLogsWithParameters(parsedUserId,
                actionType ?? ActionType.LOGIN, // Default value, will be used only if both userId and timestamp are provided
                timestamp ?? DateTime.Now);

            return PartialView("_LogsTable", logs);
        }
    }
}