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
        public IActionResult ContactUs()
        {
            return View();
        }
        public IActionResult Farms(int type)
        {
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
        //public IActionResult Chalets()
        //{
        //    ViewBag.Image = "../wwwroot/Image/chalets2.jpg";
        //    return View();
        //}
        //public IActionResult Weddings()
        //{
        //    ViewBag.Image = "../wwwroot/Image/wedding2.jpg";
        //    return View();
        //}
    }
}