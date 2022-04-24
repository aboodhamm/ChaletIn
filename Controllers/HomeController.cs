using Microsoft.AspNetCore.Mvc;

namespace Chaletin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}