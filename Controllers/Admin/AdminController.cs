using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chaletin.Controllers
{
    public class AdminController : Controller
    {
        [Authorize]
        public IActionResult Add()
        {
            return View();
        }
    }
}
