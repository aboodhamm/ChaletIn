using Chaletin.Areas.Identity.Data;
using Chaletin.Models.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chaletin.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly ChaletinDbContext _context;
        public ProfileController(ChaletinDbContext context) : base(context)
        {
            _context = context;
        }
        [Authorize]
        public IActionResult UserProfile()
        {
            var userId = GetUserId();
            var booking = _context.Booking.Where(x => x.UserId == userId && !x.Disabled).Include(x => x.Farm).ThenInclude(x => x.User).ToList();
            return View(booking);
        }
        [Authorize]
        public IActionResult AdminProfile()
        {
            ViewBag.User = _context.Users.Where(x => x.Id == GetUserId()).Select(x => x.Blocked).FirstOrDefault();
            var userId = GetUserId();
            var farms = _context.Farm.Where(x => x.UserId == userId).ToList();
            var bookings = _context.Booking.Include(x => x.Farm).Where(x => x.Farm.UserId == userId).ToList();
            var totalPayedAmount = bookings.Where(x => x.Payed).Sum(x => x.TotalAmount);
            var totalBookedFarms = bookings.Count();
            var totalAvailableFarms = farms.Where(x => x.Available).Count();
            var totalNotAvailableFarms = farms.Where(x => !x.Available).Count();
            AdminStatistics statistics = new()
            {
                TotalPayedAmount = totalPayedAmount,
                TotalBookedFarms = totalBookedFarms,
                TotalAvailableFarms = totalAvailableFarms,
                TotalNotAvailableFarms = totalNotAvailableFarms,
                TotalFarms = totalAvailableFarms + totalNotAvailableFarms
            };
            ViewBag.AdminFarms = farms;
            var x = statistics.TotalAvailableFarms / statistics.TotalFarms;
            return View(statistics);
        }

        [Authorize]
        public IActionResult SystemProfile()
        {
            ViewBag.User = _context.Users.Where(x => x.Id == GetUserId()).Select(x => x.Blocked).FirstOrDefault();
            var farms = _context.Farm.ToList();
            var bookings = _context.Booking.ToList();
            var totalFarms = farms.Count();
            var totalBookedFarms = bookings.Count();
            var totalAvailableFarms = farms.Where(x => x.Available).Count();
            var totalNotAvailableFarms = farms.Where(x => !x.Available).Count();
            AdminStatistics statistics = new()
            {
                TotalPayedAmount = totalFarms,
                TotalBookedFarms = totalBookedFarms,
                TotalAvailableFarms = totalAvailableFarms,
                TotalNotAvailableFarms = totalNotAvailableFarms,
                TotalFarms = totalAvailableFarms + totalNotAvailableFarms
            };
            ViewBag.AdminFarms = farms;
            return View(statistics);
        }

        [Authorize]
        public IActionResult Investors()
        {
            var investors = _context.Users.Where(x => _context.UserRoles.Where(x => x.RoleId == "2").Select(x => x.UserId).ToList().Contains(x.Id)).Include(x => x.Farms).ToList();
            return View(investors);
        }

        [Authorize]
        public IActionResult Clients()
        {
            var clients = _context.Users.Where(x => _context.UserRoles.Where(x => x.RoleId == "3").Select(x => x.UserId).ToList().Contains(x.Id)).ToList();
            return View(clients);
        }

        [Authorize]
        public IActionResult InvestorFarms(string id)
        {
            var farms = _context.Farm.Where(x => x.UserId == id).ToList();
            return View(farms);
        }

        [Authorize]
        public IActionResult Block(int id)
        {
            var farm = _context.Farm.Find(id);
            if (farm.Blocked)
                farm.Blocked = false;
            else
                farm.Blocked = true;
            _context.SaveChanges();
            return RedirectToAction("InvestorFarms", "Profile", new { id = farm.UserId });
        }

        [Authorize]
        public IActionResult BlockInvestor(string id)
        {
            var investor = _context.Users.Where(x => x.Id == id).FirstOrDefault();
            investor.Blocked = !investor.Blocked;
            _context.SaveChanges();
            return RedirectToAction("Investors", "Profile");
        }
        
        [Authorize]
        public IActionResult Messages()
        {
            var messages = _context.ContactMessage.Include(x => x.User).ToList();
            return View(messages);
        }

    }
}
