using ClassLibrary.Domain;
using ClassLibrary.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApi.Pages.DoctorProfile
{
    public class DoctorProfileModel : PageModel
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorProfileModel(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        [BindProperty]
        public ClassLibrary.Domain.Doctor Doctor { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!id.HasValue)
            {
                // No ID provided, handle appropriately
                TempData["ErrorMessage"] = "No doctor ID specified.";
                return RedirectToPage("/Index");
            }

            Doctor = await _doctorRepository.getDoctorByUserIdAsync(id.Value);

            if (Doctor == null)
            {
                TempData["ErrorMessage"] = "Doctor not found.";
                return RedirectToPage("/Index");
            }

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // TODO: add update logic here if UpdateDoctorAsync is implemented

            TempData["SuccessMessage"] = "Doctor profile updated successfully.";
            return RedirectToPage("Profile", new { id = Doctor.userId });
        }
    }
}