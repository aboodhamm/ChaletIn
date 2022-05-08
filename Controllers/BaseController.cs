using Chaletin.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
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
        #endregion
    }
}
