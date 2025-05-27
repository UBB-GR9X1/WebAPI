using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebClient.Models;
using System.Threading.Tasks;
using ClassLibrary.Service;

public class PatientDashboardModel : PageModel
{
    [BindProperty]
    public PatientViewModel Patient { get; set; } = new();

    private readonly IPatientService _patientService;

    public PatientDashboardModel(IPatientService patientService)
    {
        _patientService = patientService;
    }

    public async Task<IActionResult> OnGetAsync(int userId)
    {
        var success = await Patient.LoadPatientInfoByUserIdAsync(_patientService, userId);
        if (!success)
        {
            ModelState.AddModelError("", "Failed to load patient info.");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        bool hasChanges = false;

        hasChanges |= await Patient.UpdateName(_patientService);
        hasChanges |= await Patient.UpdateAddress(_patientService);
        hasChanges |= await Patient.UpdatePhoneNumber(_patientService);
        hasChanges |= await Patient.UpdateEmergencyContact(_patientService);
        hasChanges |= await Patient.UpdateWeight(_patientService);
        hasChanges |= await Patient.UpdateHeight(_patientService);
        hasChanges |= await Patient.UpdateBloodType(_patientService);
        hasChanges |= await Patient.UpdateAllergies(_patientService);
        hasChanges |= await Patient.UpdatePassword(_patientService);

        if (hasChanges)
        {
            ViewData["Message"] = "Changes applied successfully.";
        }
        else
        {
            ViewData["Message"] = "No changes detected.";
        }

        return Page();
    }
}
