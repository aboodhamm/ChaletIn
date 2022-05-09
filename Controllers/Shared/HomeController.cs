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
            var userId = GetUserId();
            var farms = _context.Farm.Where(x => x.UserId == userId).ToList();
            ViewBag.AdminFarms = farms;
            return View();
        }
        [Authorize]
        public IActionResult ContactUs()
        {
            return View();
        }
        public IActionResult Farms(int type)
        {
            if(User.IsInRole("user"))
            {

            }
            string imageSource = string.Empty;
            switch (type)
            {
                case (int)FarmType.Farm:
                    imageSource = Constants.FramUrl;
                    break;
                case (int)FarmType.Chalet:
                    imageSource = Constants.ChaletUrl;
                    break;
                case (int)FarmType.Wedding:
                    imageSource = Constants.WeddingUrl;
                    break;

            }
            ViewBag.Image = imageSource;
            return View();
        }
    }
}