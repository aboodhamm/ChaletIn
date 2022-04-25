using Microsoft.AspNetCore.Mvc;

namespace Chaletin.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
    }
}
