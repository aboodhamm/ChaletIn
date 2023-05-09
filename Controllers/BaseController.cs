using Chaletin.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Chaletin.Controllers
{
    public class BaseController : Controller
    {
        private readonly ChaletinDbContext _context;
        public BaseController(ChaletinDbContext context)
        {
            _context = context;
        }
        #region Helper
        protected string GetUserId()
        {
            var user = _context.Users.Where(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault();
            if (user is not null)
                return user.Id;
            return string.Empty;
        }
        protected User GetLoginUser()
        {
            var user = _context.Users.Where(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault();
            return user;
        }
        protected bool IsAdmin()
        {
            return _context.UserRoles.Where(x => x.RoleId == "2").Any(x => x.UserId == _context.Users.Where(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).Select(x => x.Id).FirstOrDefault());
        }
        protected bool IsSystemAdmin()
        {
            return _context.UserRoles.Where(x => x.RoleId == "1").Any(x => x.UserId == _context.Users.Where(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).Select(x => x.Id).FirstOrDefault());
        }
        protected void CheckUnpaidBooking()
        {
            var booking = _context.Booking.Where(x => !x.Payed).ToList();
            foreach (var book in booking)
            {
                if ((DateTime.Now - book.BookedDate).TotalHours > 24)
                {
                    book.Disabled = true;

                }
                _context.SaveChanges();
            }
        }
        #endregion
    }
}
