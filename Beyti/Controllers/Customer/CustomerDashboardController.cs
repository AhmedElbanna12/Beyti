using Beyti.Data;
using Beyti.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Beyti.Controllers.Customer
{

    [Authorize(Roles = "Customer")]
    public class CustomerDashboardController : Controller
    {

        private readonly BeytiDbContext _context;
        private readonly UserManager<User> _userManager;

        public CustomerDashboardController(BeytiDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // جلب كل الشيفات
            var chefs = await _userManager.GetUsersInRoleAsync("Chef");

            var chefProfiles = await _context.ChefProfiles
                .Include(cp => cp.User)
                .Where(cp => chefs.Select(u => u.Id).Contains(cp.UserId))
                .ToListAsync();

            return View(chefProfiles); // <-- لازم تمرر الـ Model للـ view
        }
    }
}

