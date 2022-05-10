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
            var booking = _context.Booking.Where(x => x.UserId == userId && !x.Disabled).Include(x => x.Farm).ToList();
            return View(booking);
        }
        [Authorize]
        public IActionResult AdminProfile()
        {
            var userId = GetUserId();
            var farms = _context.Farm.Where(x => x.UserId == userId).ToList();
            var bookings = _context.Booking.Include(x=>x.Farm).Where(x=>x.Farm.UserId == userId).ToList();
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
    }
}
