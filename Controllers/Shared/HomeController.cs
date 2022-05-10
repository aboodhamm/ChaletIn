using Chaletin.Areas.Identity.Data;
using Chaletin.Models;
using Chaletin.Models.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chaletin.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ChaletinDbContext _context;
        public HomeController(ChaletinDbContext context) : base(context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (IsAdmin())
                return RedirectToAction("AdminProfile", "Profile");
            else
                return View();
        }
        [Authorize]
        public IActionResult ContactUs()
        {
            return View();
        }
        
    }
}