using Beyti.Data;
using Beyti.Models;
using Beyti.ViewModel.Supplier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Beyti.Controllers.Supplier
{

    [Authorize(Roles = "Supplier")]
    public class SupplierController : Controller
    {
        private readonly BeytiDbContext _context;
        private readonly UserManager<User> _userManager;

        public SupplierController(BeytiDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Dashboard showing all supplies
        public async Task<IActionResult> Dashboard()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var profile = await _context.SupplierProfiles
                .Include(sp => sp.Supplies)
                .FirstOrDefaultAsync(sp => sp.UserId == userId);

            if (profile == null) return NotFound("Supplier profile not found");

            return View(profile);
        }
        // GET
        public IActionResult CreateSupply() => View(new CreateSupplyVM());

        // POST
        [HttpPost]
        public async Task<IActionResult> CreateSupply(CreateSupplyVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var profile = await _context.SupplierProfiles.FirstOrDefaultAsync(sp => sp.UserId == userId);
            if (profile == null)
            {
                ModelState.AddModelError("", "You don't have a supplier profile yet!");
                return View(vm);
            }

            var supply = new Supply
            {
                Name = vm.Name,
                Category = vm.Category,
                QualityLevel = vm.QualityLevel,
                SupplierProfileId = profile.Id
            };

            _context.Supplies.Add(supply);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Dashboard));
        }

        // GET: Edit
        public async Task<IActionResult> EditSupply(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var profile = await _context.SupplierProfiles.FirstOrDefaultAsync(sp => sp.UserId == userId);
            if (profile == null) return NotFound("Supplier profile not found");

            var supply = await _context.Supplies
                .FirstOrDefaultAsync(s => s.Id == id && s.SupplierProfileId == profile.Id);

            if (supply == null) return NotFound("This supply does not belong to you!");

            var vm = new EditSupplyVM
            {
                Id = supply.Id,
                Name = supply.Name,
                Category = supply.Category,
                QualityLevel = supply.QualityLevel
            };

            return View(vm);
        }

        // POST: Edit
        [HttpPost]
        public async Task<IActionResult> EditSupply(EditSupplyVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var profile = await _context.SupplierProfiles.FirstOrDefaultAsync(sp => sp.UserId == userId);
            if (profile == null) return NotFound("Supplier profile not found");

            var supply = await _context.Supplies
                .FirstOrDefaultAsync(s => s.Id == vm.Id && s.SupplierProfileId == profile.Id);

            if (supply == null) return BadRequest("This supply does not belong to you!");

            // تحديث الحقول
            supply.Name = vm.Name;
            supply.Category = vm.Category;
            supply.QualityLevel = vm.QualityLevel;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Dashboard));
        }


        // POST: Delete
        [HttpPost]
        public async Task<IActionResult> DeleteSupply(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var profile = await _context.SupplierProfiles.FirstOrDefaultAsync(sp => sp.UserId == userId);
            if (profile == null) return NotFound();

            var supply = await _context.Supplies
                .FirstOrDefaultAsync(s => s.Id == id && s.SupplierProfileId == profile.Id);

            if (supply != null)
            {
                _context.Supplies.Remove(supply);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Dashboard));
        }
    }
}
