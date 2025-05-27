using Microsoft.AspNetCore.Mvc;
using ClassLibrary.Service;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ClassLibrary.Model; // For shared interfaces

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

        // GET: Patient/Profile/5
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
                TempData["Error"] = "Unable to determine the patient profile to display.";
                return RedirectToAction("Index", "Home");
            }

            var success = await _patientService.loadPatientInfoByUserId(userId.Value);
            if (!success)
            {
                TempData["Error"] = "Patient not found or error loading information.";
                return RedirectToAction("Index", "Home");
            }

            return View(_patientService.patientInfo);
        }

        // GET: Patient/Edit/5
        public async Task<IActionResult> Edit(int? id = null)
        {
            if (!id.HasValue && User.Identity.IsAuthenticated)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int currentUserId))
                {
                    id = currentUserId;
                }
            }

            if (!id.HasValue)
            {
                TempData["Error"] = "Unable to determine the patient to edit.";
                return RedirectToAction("Index", "Home");
            }

            var success = await _patientService.loadPatientInfoByUserId(id.Value);
            if (!success)
            {
                TempData["Error"] = "Patient not found or error loading data.";
                return RedirectToAction("Index", "Home");
            }

            return View(_patientService.patientInfo);
        }

        // POST: Patient/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientId,PatientName,Address,PhoneNumber,Weight,Height,EmergencyContact,BloodType,Allergies")] PatientJointModel patient)
        {
            if (id != patient.patientId)
            {
                TempData["Error"] = "Invalid patient ID.";
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                bool updateSuccess = true;

                // Use individual update methods based on the fields that were changed
                if (!string.IsNullOrEmpty(patient.phoneNumber))
                {
                    updateSuccess &= await _patientService.updateName(id, patient.phoneNumber);
                }
                if (!string.IsNullOrEmpty(patient.address))
                {
                    updateSuccess &= await _patientService.updateAddress(id, patient.address);
                }
                if (!string.IsNullOrEmpty(patient.phoneNumber))
                {
                    updateSuccess &= await _patientService.updatePhoneNumber(id, patient.phoneNumber);
                }
                if (patient.weight > 0)
                {
                    updateSuccess &= await _patientService.updateWeight(id, patient.weight);
                }
                if (patient.height > 0)
                {
                    updateSuccess &= await _patientService.updateHeight(id, patient.height);
                }
                if (!string.IsNullOrEmpty(patient.emergencyContact))
                {
                    updateSuccess &= await _patientService.updateEmergencyContact(id, patient.emergencyContact);
                }
                if (!string.IsNullOrEmpty(patient.bloodType))
                {
                    updateSuccess &= await _patientService.updateBloodType(id, patient.bloodType);
                }
                if (!string.IsNullOrEmpty(patient.allergies))
                {
                    updateSuccess &= await _patientService.updateAllergies(id, patient.allergies);
                }

                if (updateSuccess)
                {
                    TempData["Success"] = "Profile updated successfully.";
                    return RedirectToAction("Profile", new { userId = id });
                }
                else
                {
                    TempData["Error"] = "Failed to update profile.";
                }
            }

            return View(patient);
        }
    }
}