using Chaletin.Areas.Identity.Data;
using Chaletin.Models;
using Chaletin.Models.Enum;
using Chaletin.Models.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult Index(int type, string imageSrc, int bedRoomCount, int capacity, int price, int city)
        {
            var farmsToCheck = _context.Booking.Include(x => x.Farm).OrderByDescending(x=>x.From).ToList();
            var today = DateTime.Now;
            foreach (var item in farmsToCheck)
            {
                int conflict = 0;
                if (item.From <= today && item.To >= today)
                    conflict++;
                if (conflict == 0)
                    item.Farm.Booked = false;
                else
                    item.Farm.Booked = true;
                _context.SaveChanges();
            }

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

            var farms = _context.Farm.Where(x => x.Type == type && x.Available &&
                        (bedRoomCount != 0 ? x.BedRoomCount == bedRoomCount : x.BedRoomCount > 0) &&
                        //((capacity != 0) ? (capacity > 30) ? (x.Capacity > 30) ? (capacity > 20 && capacity <= 30) : (x.Capacity > 20 && x.Capacity <= 30) ? (capacity> 10 && capacity <= 20) : (x.Capacity > 10 && x.Capacity <= 20) ? (capacity > 0 && capacity <= 10) : (x.Capacity > 0 && x.Capacity <= 10) : (x.Capacity > 0) &&
                        //((price != 0) ? (price >= 300) ? (x.Price > 300) : (price > 100 && price <= 200) ? (x.Price > 100 && x.Price <= 200) : (price > 0 && price <= 100) ? (x.Price > 0 && x.Price <= 100) : (x.Price > 0) &&
                        (city != 0 ? x.City == city : x.City > 0)
                        ).Select(x => new FarmViewModel
                        {
                            Id = x.Id,
                            Title = x.Title,
                            //ImageSource = x.ImageSource,
                            Type = (FarmType)x.Type,
                            City = (City)x.City,
                            LivingRoomCount = x.LivingRoomCount,
                            BedRoomCount = x.BedRoomCount,
                            BathRoomCount = x.BathRoomCount,
                            Description = x.Description,
                            Rate = x.Rate,
                            Price = x.Price,
                            Booked = x.Booked
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
            ViewBag.Type = type;
            var UserRole = _context.UserRoles.Any(x => x.UserId == GetUserId());
            return View(farms);
        }
        public IActionResult FarmDetails(int id)
        {
            var farm = _context.Farm.Find(id);
            var comments = _context.Comments.Where(x => x.FarmId == id).OrderBy(x=>x.Id).Include(x => x.User).ToList();
            var result = new FarmDetailsViewModel()
            {
                Id = farm.Id,
                Title = farm.Title,
                //ImageSource = farm.ImageSource,
                Type = (FarmType)farm.Type,
                City = (City)farm.City,
                LivingRoomCount = farm.LivingRoomCount,
                BedRoomCount = farm.BedRoomCount,
                BathRoomCount = farm.BathRoomCount,
                Description = farm.Description,
                Rate = farm.Rate,
                Price = farm.Price,
                LivingRoomDescription = farm.LivingRoomDescription,
                SwimmingPoolDescription = farm.SwimmingPoolDescription,
                BedRoomDescription = farm.BedRoomDescription,
                BathRoomDescription = farm.BathRoomDescription,
                KitchenDescription = farm.KitchenDescription,
                PublicUtilityDescription = farm.PublicUtilityDescription,
                Booked = farm.Booked
            };
            result.Comments = comments;
            return View(result);
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
                UserId = userId,
                Title = model.Title,
                Price = model.Price,
                Rate = model.Rate,
                //ImageSource = model.ImageSource,
                Type = (int)model.Type,
                City = (int)model.City,
                LivingRoomCount = model.LivingRoomCount,
                BedRoomCount = model.BedRoomCount,
                BathRoomCount = model.BathRoomCount,
                Description = model.Description,
                PublicUtilityDescription = model.PublicUtilityDescription,
                LivingRoomDescription = model.LivingRoomDescription,
                BedRoomDescription = model.BedRoomDescription,  
                BathRoomDescription= model.BathRoomDescription,
                SwimmingPoolDescription = model.SwimmingPoolDescription,
                KitchenDescription = model.KitchenDescription,
                Available = true
            };

            _context.Farm.Add(farm);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Booking(int id,string errorMessage = "")
        {
            var farm = _context.Farm.Find(id);
            var result = new BookingViewModel
            {
                Id = farm.Id,
                FarmTitle = farm.Title,
                //ImageSource = farm.ImageSource,
                Type = (FarmType)farm.Type,
                City = (City)farm.City,
                LivingRoomCount = farm.LivingRoomCount,
                BedRoomCount = farm.BedRoomCount,
                BathRoomCount = farm.BathRoomCount,
                Description = farm.Description,
                Rate = farm.Rate,
                Price = farm.Price,
                ErrorMessage = errorMessage
            };
            return View(result);
        }

        [Authorize]
        public IActionResult ConfirmBooking(BookingViewModel model)
        {
            var existingBooking = _context.Booking.Where(x => x.FarmId == model.Id).ToList();
            foreach(var item in existingBooking)
            {
                if ((model.From.Date <= item.From.Date && model.To.Date >= item.To.Date) || (model.From.Date >= item.From.Date && model.To.Date <= item.To.Date) ||
                    (model.From.Date <= item.To.Date && model.To.Date >= item.To.Date) || (model.From.Date <= item.From.Date && model.To.Date >= model.From.Date))
                {
                    var errorMessage = "تاريخ الحجز غير متوفر*";
                    return RedirectToAction("Booking", "Farm", new { model.Id,errorMessage });
                }
            }
            
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
                Payed = !model.PayLater,
                Disabled = false
            };
            model.TotalAmount = totalAmount;

            if (model.From <= DateTime.Now && model.To >= DateTime.Now)
            {
                var farm = _context.Farm.Find(model.Id);
                farm.Booked = true;
            }
            
            _context.Booking.Add(booking);
            _context.SaveChanges();
            if (model.PayLater)
            {
                return RedirectToAction("UserProfile", "Profile");
            }
            else
            {
                return RedirectToAction("Pay", "Farm", new { id = booking.Id });
            }
        }

        [Authorize]
        public IActionResult Pay(int id)
        {
            var booking = _context.Booking.Find(id);
            var result = new BookingViewModel()
            {
                Id = id,
                TotalAmount = booking.TotalAmount
            };
            return View(result);
        }

        [Authorize]
        public IActionResult ConfirmPayment(int id)
        {
            var booking = _context.Booking.Find(id);
            if (booking is not null)
            {
                booking.Payed = true;
                _context.SaveChanges();
            }

            return RedirectToAction("UserProfile", "Profile");
        }

        [Authorize]
        public IActionResult DisableBooking(int id)
        {
            var booking = _context.Booking.Find(id);
            if (booking is not null)
            {
                booking.Disabled = true;
                _context.SaveChanges();
            }
            return RedirectToAction("Profile", "User");
        }

        [Authorize]
        public IActionResult Test()
        {
            if(_context.UserRoles.Where(x=>x.RoleId == "1").Any(x => x.UserId == _context.Users.Where(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).Select(x=>x.Id).FirstOrDefault()))
            {

            }
            if (_context.UserRoles.Where(x => x.RoleId == "2").Any(x => x.UserId == GetUserId()))
            {

            }
                return RedirectToAction("index", "Farm");
        }
        public IActionResult Dashboard()
        {
            return View();
        }
       
        [Authorize]
        public IActionResult AddComment(int farmId,string comment)
        {
            var userId = GetUserId();
            Comments result = new()
            {
                UserId = userId,
                FarmId = farmId,
                Comment = comment,
                CreatedDate = DateTime.Now
            };
            _context.Comments.Add(result);
            _context.SaveChanges();
            return RedirectToAction("FarmDetails", "Farm",new {id = farmId});
        }

        public IActionResult AddRate(int rate, int farmId)
        {
            var farm = _context.Farm.Where(x => x.Id == farmId).FirstOrDefault();
            var currentRate = farm.Rate;
            if (currentRate == 5 && (rate < currentRate))
                currentRate -= 0.5;
            else if ((currentRate == 1 || currentRate == 0) && (rate > currentRate))
                currentRate += 0.5;
            else if (currentRate > rate)
                currentRate -= 0.5;
            else if (currentRate < rate)
                currentRate += 0.5;
            farm.Rate = currentRate;
            _context.SaveChanges();
            return RedirectToAction("FarmDetails", "Farm", new { id = farmId });
        }
        public IActionResult ChangeAvailability(int id)
        {
            var farm = _context.Farm.Find(id);
            if (farm.Available)
                farm.Available = false;
            else
                farm.Available = true;
            _context.SaveChanges();
            return RedirectToAction("AdminProfile", "Profile");
        }
    }
}
