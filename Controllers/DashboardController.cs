using Microsoft.AspNetCore.Mvc;

namespace AgriEnergyConnect.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult EmployeeDashboard()
        {
            // Fetch necessary data and pass to the view
            return View();
        }
    }
}
