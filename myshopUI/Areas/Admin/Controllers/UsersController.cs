using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myshop.DataAccess;
using System.Security.Claims;
using Utilities;


namespace myshopUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles= SD.AdminRole)]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userID = claim.Value;
            return View(_context.ApplicationUsers.Where(x => x.Id != userID).ToList());
        }
    }
}
