using Chaletin.Models;
using Chaletin.Models.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chaletin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
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