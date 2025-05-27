using System.Diagnostics;
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
        public async Task<IActionResult> Edit(int id, [Bind("patientId,userId,patientName,address,phoneNumber,weight,height,emergencyContact,bloodType,allergies")] PatientJointModel patient)
        {
            if (id != patient.userId)
            {
                TempData["Error"] = $"Invalid user ID.";
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                bool updateSuccess = true;

                if (!string.IsNullOrEmpty(patient.patientName))
                {
                    bool result = await _patientService.updateName(id, patient.patientName);
                    updateSuccess &= result;
                }
                if (!string.IsNullOrEmpty(patient.address))
                {
                    bool result = await _patientService.updateAddress(id, patient.address);
                    updateSuccess &= result;
                }
                if (!string.IsNullOrEmpty(patient.phoneNumber))
                {
                    bool result = await _patientService.updatePhoneNumber(id, patient.phoneNumber);
                    updateSuccess &= result;
                }
                if (patient.weight > 0)
                {
                    bool result = await _patientService.updateWeight(id, patient.weight);
                    updateSuccess &= result;
                }
                if (patient.height > 0)
                {
                    bool result = await _patientService.updateHeight(id, patient.height);
                    updateSuccess &= result;
                }
                if (!string.IsNullOrEmpty(patient.emergencyContact))
                {
                    bool result = await _patientService.updateEmergencyContact(id, patient.emergencyContact);
                    updateSuccess &= result;
                }
                if (!string.IsNullOrEmpty(patient.bloodType))
                {
                    bool result = await _patientService.updateBloodType(id, patient.bloodType);
                    updateSuccess &= result;
                }
                if (!string.IsNullOrEmpty(patient.allergies))
                {
                    bool result = await _patientService.updateAllergies(id, patient.allergies);
                    updateSuccess &= result;
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