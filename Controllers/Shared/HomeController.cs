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
        [Authorize]
        public IActionResult Index()
        {
            CheckUnpaidBooking();
            if (IsAdmin())
                return RedirectToAction("AdminProfile", "Profile");
            else if (IsSystemAdmin())
            {
                return RedirectToAction("SystemProfile", "Profile");
            }
            else
                return View();
        }

        [Authorize]
        public IActionResult ContactUs()
        {
            return View();
        }

        [Authorize]
        public IActionResult AddMessage(ContactMessage model)
        {
            var user = GetLoginUser();
            model.UserId = user.Id;
            _context.ContactMessage.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

    }
}