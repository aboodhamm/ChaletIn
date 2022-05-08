using Chaletin.Areas.Identity.Data;
using Chaletin.Models;
using Chaletin.Models.Enum;
using Chaletin.Models.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Chaletin.Controllers
{
    public class FarmController : BaseController
    {
        private readonly ChaletinDbContext _context;
        public FarmController(ChaletinDbContext context) : base(context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Add()
        {
            return View();
        }
        [Authorize]
        public IActionResult AddFarm(FarmDtoModel model)
        {
            var userId = GetUserId();

            Farm farm = new()
            {
                Title = model.Title,
                Price = model.Price,
                Rate = model.Rate,
                ImageSource = model.ImageSource,
                Type = (int)model.Type,
                City = (int)model.City,
                LivingRoomCount = model.LivingRoomCount,
                BedRoomCount = model.BedRoomCount,
                BathRoomCount = model.BathRoomCount,
                Description = model.Description,
                UserId = userId
            };

            _context.Farm.Add(farm);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Index(int type, string imageSrc, int bedRoomCount, int capacity, int price, int city)
        {
            string imageSource = string.Empty;
            if (imageSrc is not null)
            {
                imageSource = imageSrc;
            }
            else
            {
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
            }
            var farms = _context.Farm.Where(x => (bedRoomCount == 0 || x.BedRoomCount == bedRoomCount) &&
                        (capacity != 0 ? (capacity == 400 ? x.Capacity > 400 : x.Capacity > (capacity - 100) && x.Capacity <= capacity) : x.Capacity > 0) &&
                        (price != 0 ? (price == 400 ? x.Price > 400 : x.Price > (price - 100) && x.Price <= price) : x.Price > 0) &&
                        (city == 0 || x.City == city)
                        ).Select(x => new FarmViewModel
                        {
                            Id = x.Id,
                            Title = x.Title,
                            ImageSource = x.ImageSource,
                            Type = (FarmType)x.Type,
                            City = (City)x.City,
                            LivingRoomCount = x.LivingRoomCount,
                            BedRoomCount = x.BedRoomCount,
                            BathRoomCount = x.BathRoomCount,
                            Description = x.Description,
                            Rate = x.Rate,
                            Price = x.Price
                        }).ToList();
            var City = Enum.GetValues(typeof(City)).Cast<City>().ToList();
            Dictionary<int, string> cities = new();
            var i = 1;
            foreach (var x in City)
            {
                cities.Add(i, x.ToString());
                i++;
            }
            ViewBag.Image = imageSource;
            ViewBag.Cities = cities;
            return View(farms);
        }
        public IActionResult Booking(int id)
        {
            var farm = _context.Farm.Find(id);
            var result = new FarmViewModel
            {
                Id = farm.Id,
                Title = farm.Title,
                ImageSource = farm.ImageSource,
                Type = (FarmType)farm.Type,
                City = (City)farm.City,
                LivingRoomCount = farm.LivingRoomCount,
                BedRoomCount = farm.BedRoomCount,
                BathRoomCount = farm.BathRoomCount,
                Description = farm.Description,
                Rate = farm.Rate,
                Price = farm.Price
            };
            return View(result);
        }
        [Authorize]
        public IActionResult ConfirmBooking(FarmViewModel model)
        {
            var period = (model.To - model.From).Days;
            var totalAmount = period * model.Price;
            var userId = GetUserId();
            Booking booking = new()
            {
                UserId = userId,
                FarmId = model.Id,
                Price = model.Price,
                TotalAmount = totalAmount,
                From = model.From,
                To = model.To,
                Period = period,
                Payed = model.PayLater,
                Disabled = false
            };
            model.TotalAmount = totalAmount;
            _context.Booking.Add(booking);
            _context.SaveChanges();
            if (model.PayLater)
            {
                return RedirectToAction("Profile","User");
            }
            else
            {
                return RedirectToAction("Pay",booking.Id);
            }
        }
        public IActionResult Pay(int id)
        {
            var booking = _context.Booking.Find(id);
            
            return View();
        }
        public IActionResult ConfirmPayment()
        {
            return RedirectToAction("Profile", "User");
        }
    }
}
