using Chaletin.Areas.Identity.Data;
using Chaletin.Models;
using Chaletin.Models.Enum;
using Chaletin.Models.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Chaletin.Controllers
{
    public class FarmController : BaseController
    {
        private readonly ChaletinDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FarmController(ChaletinDbContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(int type, string imageSrc, int bedRoomCount, int capacity, int price, int city)
        {
            //CheckFarmsAvailability();
            string imageSource = string.Empty;
            string pageTitle = string.Empty;
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
                        pageTitle = Constants.FramTitle;
                        break;
                    case (int)FarmType.Chalet:
                        imageSource = Constants.ChaletUrl;
                        pageTitle = Constants.ChaletTitle;
                        break;
                    case (int)FarmType.Wedding:
                        imageSource = Constants.WeddingUrl;
                        pageTitle = Constants.WeddingTitle;
                        break;

                }
            }

            var farms = GetFilteredFrams(type, bedRoomCount, price, capacity, city);
            var City = Enum.GetValues(typeof(City)).Cast<City>().ToList();
            Dictionary<int, string> cities = new();
            var i = 1;
            foreach (var x in City)
            {
                cities.Add(i, x.ToString());
                i++;
            }

            PageComponent component = new()
            {
                Image = imageSource,
                City = cities,
                Type = type,
                Title = pageTitle
            };

            ViewBag.PageComponent = component;
            var UserRole = _context.UserRoles.Any(x => x.UserId == GetUserId());
            return View(farms);
        }

        public IActionResult FarmDetails(int id)
        {
            //CheckFarmsAvailability();
            var farm = _context.Farm.Find(id);
            var comments = _context.Comments.Where(x => x.FarmId == id).OrderBy(x => x.Id).Include(x => x.User).ToList();
            var result = new FarmDetailsViewModel()
            {
                Id = farm.Id,
                Title = farm.Title,
                Images = farm.Images.ToList(),
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
            ViewBag.User = _context.Users.Where(x => x.Id == GetUserId()).Select(x => x.Blocked).FirstOrDefault();
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
                Type = (int)model.Type,
                City = (int)model.City,
                LivingRoomCount = model.LivingRoomCount,
                BedRoomCount = model.BedRoomCount,
                BathRoomCount = model.BathRoomCount,
                Description = model.Description,
                PublicUtilityDescription = model.PublicUtilityDescription,
                LivingRoomDescription = model.LivingRoomDescription,
                BedRoomDescription = model.BedRoomDescription,
                BathRoomDescription = model.BathRoomDescription,
                SwimmingPoolDescription = model.SwimmingPoolDescription,
                KitchenDescription = model.KitchenDescription,
                Available = true
            };
            farm.Images = UploadFile(model.ImagesFiles).ToArray();
            _context.Farm.Add(farm);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        private List<string> UploadFile(List<IFormFile> images)
        {
            List<string> result = new();
            foreach (var image in images)
            {
                string Directory = Path.Combine(_webHostEnvironment.WebRootPath, "FarmImages");
                string fileName = Guid.NewGuid().ToString() + "-" + image.FileName;
                string filePath = Path.Combine(Directory, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
                result.Add(fileName);
            }
            return result;
        }

        [Authorize]
        public IActionResult Booking(int id, string errorMessage = "")
        {
            var farm = _context.Farm.Find(id);
            var result = new BookingViewModel
            {
                Id = farm.Id,
                FarmTitle = farm.Title,
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
            ViewBag.FarmBookingDetails = _context.Booking.Where(x => x.FarmId == id)
                                            .Select(a => new FarmBookingDetails
                                            {
                                                From = a.From,
                                                To = a.To
                                            }).ToList();
            return View(result);
        }

        [Authorize]
        public IActionResult ConfirmBooking(BookingViewModel model)
        {
            var existingBooking = _context.Booking.Where(x => x.FarmId == model.Id).ToList();
            foreach (var item in existingBooking)
            {
                if ((model.From.Date <= item.From.Date && model.To.Date >= item.To.Date) || (model.From.Date >= item.From.Date && model.To.Date <= item.To.Date) ||
                    (model.From.Date < item.To.Date && model.To.Date >= item.To.Date) || (model.From.Date < item.From.Date && model.To.Date > item.From.Date))
                {
                    var errorMessage = "تاريخ الحجز غير متوفر*";
                    return RedirectToAction("Booking", "Farm", new { model.Id, errorMessage });
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
                Disabled = false,
                BookedDate = DateTime.Now
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
                _context.Remove(booking);
                _context.SaveChanges();
            }
            return RedirectToAction("UserProfile", "Profile");
        }

        [Authorize]
        public IActionResult AddComment(int farmId, string comment)
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
            return RedirectToAction("FarmDetails", "Farm", new { id = farmId });
        }

        [Authorize]
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

        [Authorize]
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

        private List<FarmViewModel> GetFilteredFrams(int type, int bedRoomCount, int price, int capacity, int city)
        {
            var request = _context.Farm.Where(x => x.Type == type && x.Available &&
                        (bedRoomCount != 0 ? x.BedRoomCount == bedRoomCount : x.BedRoomCount > 0) &&
                        (city != 0 ? x.City == city : x.City > 0)).ToList();

            if (price > 0 && price <= 100)
                request = request.Where(x => x.Price > 0 && x.Price <= 100).ToList();
            else if (price >= 100 && price <= 200)
                request = request.Where(x => x.Price >= 100 && x.Price <= 200).ToList();
            else if (price >= 200 && price <= 300)
                request = request.Where(x => x.Price >= 200 && x.Price <= 300).ToList();
            else if (price >= 300)
                request = request.Where(x => x.Price >= 300).ToList();

            if (capacity > 0 && capacity <= 10)
                request = request.Where(x => x.Capacity >= 0 && x.Capacity <= 10).ToList();
            else if (capacity >= 10 && capacity <= 20)
                request = request.Where(x => x.Capacity >= 10 && x.Capacity <= 20).ToList();
            else if (capacity >= 20 && capacity <= 30)
                request = request.Where(x => x.Capacity >= 20 && x.Capacity <= 30).ToList();
            else if (capacity >= 30)
                request = request.Where(x => x.Capacity >= 300).ToList();

            var result = request.Select(x => new FarmViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Images = x.Images.ToList(),
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

            return result;

        }
    }
}
