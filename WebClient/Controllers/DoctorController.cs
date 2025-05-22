using ClassLibrary.Domain;
using ClassLibrary.Model;
using ClassLibrary.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebClient.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorController : Controller
    {
        public async Task<IActionResult> Dashboard()
        {
            //var model = new DoctorFilterViewModel
            //{
            //    doctors = await _doctorService.getAllDoctors(),
            //    selected_date = DateTime.Now
            //};
            return View();
        }

    }
}
