using Microsoft.AspNetCore.Mvc;

namespace TeknoJobs.Controllers
{
    public class LocationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
