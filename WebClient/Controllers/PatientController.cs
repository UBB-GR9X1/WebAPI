using Microsoft.AspNetCore.Mvc;
using ClassLibrary.Service;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims; // For shared interfaces

namespace WebClient.Controllers
{
    [Authorize(Roles = "Patient")]
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly IAuthService _authService;

        public PatientController(IPatientService patientService, IAuthService authService)
        {
            _patientService = patientService;
            _authService = authService;
        }

        public async Task<IActionResult> Profile(int? userId = null)
        {
            if (!userId.HasValue && User.Identity.IsAuthenticated)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int currentUserId))
                {
                    userId = currentUserId;
                }
            }

            if (!userId.HasValue)
            {
                TempData["Error"] = "Unable to determine the doctor profile to display.";
                return RedirectToAction("Index", "Home");
            }

            var success = await _patientService.loadPatientInfoByUserId(userId.Value);
            if (!success)
            {
                TempData["Error"] = "Doctor not found or error loading information.";
                return RedirectToAction("Index", "Home");
            }

            return View(_patientService.patientInfo);
        }
    }
}