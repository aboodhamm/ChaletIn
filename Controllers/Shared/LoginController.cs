using Microsoft.AspNetCore.Mvc;

namespace Chaletin.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
