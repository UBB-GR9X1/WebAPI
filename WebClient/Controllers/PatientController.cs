using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ClassLibrary.Model;
using ClassLibrary.Service;

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

        private int? GetCurrentUserId()
        {
            if (User.Identity.IsAuthenticated)
            {
                var claim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (int.TryParse(claim?.Value, out int userId))
                    return userId;
            }
            return null;
        }

        // GET: Patient/Profile/5
        public async Task<IActionResult> Profile(int? userId = null)
        {
            userId ??= GetCurrentUserId();

            if (!userId.HasValue)
            {
                TempData["Error"] = "Unable to determine the patient profile to display.";
                return RedirectToAction("Index", "Home");
            }

            var success = await _patientService.loadPatientInfoByUserId(userId.Value);
            if (!success || _patientService.patientInfo == PatientJointModel.Default)
            {
                TempData["Error"] = "Patient not found or error loading information.";
                return RedirectToAction("Index", "Home");
            }

            return View(_patientService.patientInfo);
        }

        // GET: Patient/Edit/5
        public async Task<IActionResult> Edit(int? id = null)
        {
            id ??= GetCurrentUserId();

            if (!id.HasValue)
            {
                TempData["Error"] = "Unable to determine the patient to edit.";
                return RedirectToAction("Index", "Home");
            }

            var success = await _patientService.loadPatientInfoByUserId(id.Value);
            if (!success || _patientService.patientInfo == PatientJointModel.Default)
            {
                TempData["Error"] = "Patient not found or error loading data.";
                return RedirectToAction("Index", "Home");
            }

            return View(_patientService.patientInfo);
        }

        // POST: Patient/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("patientId,patientName,address,phoneNumber,weight,height,emergencyContact,bloodType,allergies,username,email,birthDate,cnp,userId,password")] PatientJointModel patient)
        {
            // Debug logging
            System.Diagnostics.Debug.WriteLine($"POST Edit - Route ID: {id}, Patient.userId: {patient.userId}, Patient.patientId: {patient.patientId}");
            
            // Use the current user ID instead of relying on route parameter
            var currentUserId = GetCurrentUserId();
            if (!currentUserId.HasValue)
            {
                TempData["Error"] = "Unable to determine current user.";
                return RedirectToAction("Index", "Home");
            }
            
            // Use the current user ID for validation and updates
            id = currentUserId.Value;

            if (!ModelState.IsValid)
                return View(patient);

            var existingSuccess = await _patientService.loadPatientInfoByUserId(id);
            if (!existingSuccess || _patientService.patientInfo == PatientJointModel.Default)
            {
                TempData["Error"] = "Patient not found.";
                return RedirectToAction("Index", "Home");
            }

            var existing = _patientService.patientInfo;
            bool updateSuccess = true;

            if (existing.patientName != patient.patientName)
                updateSuccess &= await _patientService.updateName(id, patient.patientName);

            if (existing.address != patient.address)
                updateSuccess &= await _patientService.updateAddress(id, patient.address);

            if (existing.phoneNumber != patient.phoneNumber)
                updateSuccess &= await _patientService.updatePhoneNumber(id, patient.phoneNumber);

            if (existing.weight != patient.weight && patient.weight > 0)
                updateSuccess &= await _patientService.updateWeight(id, patient.weight);

            if (existing.height != patient.height && patient.height > 0)
                updateSuccess &= await _patientService.updateHeight(id, patient.height);

            if (existing.emergencyContact != patient.emergencyContact)
                updateSuccess &= await _patientService.updateEmergencyContact(id, patient.emergencyContact);

            if (existing.bloodType != patient.bloodType)
                updateSuccess &= await _patientService.updateBloodType(id, patient.bloodType);

            if (existing.allergies != patient.allergies)
                updateSuccess &= await _patientService.updateAllergies(id, patient.allergies);

            if (existing.email != patient.email)
                updateSuccess &= await _patientService.updateEmail(id, patient.email);

            if (existing.username != patient.username)
                updateSuccess &= await _patientService.updateUsername(id, patient.username);

            if (existing.cnp != patient.cnp)
                updateSuccess &= await _patientService.updateCnp(id, patient.cnp);

            if (updateSuccess)
            {
                TempData["Success"] = "Profile updated successfully.";
                return RedirectToAction("Profile", new { userId = id });
            }

            ModelState.AddModelError("", "Failed to update profile.");
            return View(patient);
        }
    }
}
