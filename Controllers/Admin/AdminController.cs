using Microsoft.AspNetCore.Mvc;

namespace Chaletin.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Add()
        {
            return View();
        }
    }
}
