using Microsoft.AspNetCore.Mvc;

namespace WebClient.Controllers
{
    public class DoctorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
