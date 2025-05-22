using ClassLibrary.Domain;
using ClassLibrary.Model;
using ClassLibrary.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WebClient.Models;

namespace WebClient.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorController : Controller
    {

        private readonly IDoctorService _doctorService;
        private readonly IAuthService _authService;

        public DoctorController(IDoctorService doctorService, IAuthService authService)
        {
            _doctorService = doctorService;
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

            var success = await _doctorService.LoadDoctorInformationByUserId(userId.Value);
            if (!success)
            {
                TempData["Error"] = "Doctor not found or error loading information.";
                return RedirectToAction("Index", "Home");
            }

            return View(_doctorService.DoctorInformation);
        }

        public async Task<IActionResult> Edit(int? userId = null)
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
                TempData["Error"] = "Unable to determine which doctor profile to edit.";
                return RedirectToAction("Index", "Home");
            }

            var success = await _doctorService.LoadDoctorInformationByUserId(userId.Value);
            if (!success)
            {
                TempData["Error"] = "Doctor not found or error loading information.";
                return RedirectToAction("Index", "Home");
            }

            var departments = await _doctorService.GetAllDepartments();
            ViewBag.Departments = new SelectList(departments, "departmentId", "departmentName",
                _doctorService.DoctorInformation.DepartmentId);

            return View(_doctorService.DoctorInformation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DoctorModel model)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _doctorService.GetAllDepartments();
                ViewBag.Departments = new SelectList(departments, "departmentId", "departmentName", model.DepartmentId);
                return View(model);
            }

            try
            {
                await _doctorService.LoadDoctorInformationByUserId(model.DoctorId);
                var currentDoctor = _doctorService.DoctorInformation;

                bool updateSuccess = true;

                if (currentDoctor.DoctorName != model.DoctorName)
                {
                    updateSuccess &= await _doctorService.UpdateDoctorProfile(model.DoctorId,
                        DoctorService.UpdateField.DoctorName, model.DoctorName);
                }

                if (currentDoctor.DepartmentId != model.DepartmentId)
                {
                    updateSuccess &= await _doctorService.UpdateDoctorProfile(model.DoctorId,
                        DoctorService.UpdateField.Department, "", model.DepartmentId);
                }

                if (currentDoctor.CareerInfo != model.CareerInfo)
                {
                    updateSuccess &= await _doctorService.UpdateDoctorProfile(model.DoctorId,
                        DoctorService.UpdateField.CareerInfo, model.CareerInfo);
                }

                if (currentDoctor.PhoneNumber != model.PhoneNumber)
                {
                    updateSuccess &= await _doctorService.UpdateDoctorProfile(model.DoctorId,
                        DoctorService.UpdateField.PhoneNumber, model.PhoneNumber);
                }

                if (currentDoctor.Mail != model.Mail)
                {
                    updateSuccess &= await _doctorService.UpdateDoctorProfile(model.DoctorId,
                        DoctorService.UpdateField.Mail, model.Mail);
                }

                if (currentDoctor.AvatarUrl != model.AvatarUrl)
                {
                    updateSuccess &= await _doctorService.UpdateDoctorProfile(model.DoctorId,
                        DoctorService.UpdateField.AvatarUrl, model.AvatarUrl);
                }

                if (updateSuccess)
                {
                    TempData["Success"] = "Doctor profile updated successfully!";
                    return RedirectToAction("Profile", new { userId = model.DoctorId });
                }
                else
                {
                    TempData["Error"] = "Error updating doctor profile.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error updating profile: {ex.Message}";
            }

            var departmentList = await _doctorService.GetAllDepartments();
            ViewBag.Departments = new SelectList(departmentList, "departmentId", "departmentName", model.DepartmentId);
            return View(model);
        }
    }
}
