using Chaletin.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chaletin.Controllers
{
    public class UserController : BaseController
    {
        private readonly ChaletinDbContext _context;
        public UserController(ChaletinDbContext context) : base(context)
        {
            _context = context;
        }
        [Authorize]
        public IActionResult Profile()
        {
            var userId = GetUserId();
            var booking = _context.Booking.Where(x => x.UserId == userId && !x.Disabled).Include(x => x.Farm).ToList();
            return View(booking);
        }
    }
}
