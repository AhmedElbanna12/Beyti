using Beyti.Data;
using Beyti.Models;
using Beyti.ViewModel.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Beyti.Controllers
{
        [Authorize]
        public class ProfileController : Controller
        {
            private readonly UserManager<User> _userManager;
            private readonly BeytiDbContext _context;

            public ProfileController(UserManager<User> userManager, BeytiDbContext context)
            {
                _userManager = userManager;
                _context = context;
            }

            // GET: /Profile/
            public async Task<IActionResult> Index()
            {

            var userId = int.Parse(_userManager.GetUserId(User));

            var user = await _context.Users
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Id == userId);

            // باقي الكود زي ما هو
            if (user == null) return RedirectToAction("Login", "Auth");

                // مثال: جلب البيانات الخاصة بالمستخدم من الـ DB
                var profileVM = new ProfileVM
                {
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault(),
                    City = user.Address.City,
                    Street = user.Address.Street,
                    BuildingNo = user.Address.BuildingNo,
                    Floor = user.Address.Floor
                };

                return View(profileVM);
            }

            // GET: /Profile/Edit
            public async Task<IActionResult> Edit()
            {
            var userId = int.Parse(_userManager.GetUserId(User));

            var user = await _context.Users
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Id == userId);
            // باقي الكود زي ما هو
            if (user == null) return RedirectToAction("Login", "Auth");

                var profileVM = new ProfileVM
                {
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    City = user.Address.City,
                    Street = user.Address.Street,
                    BuildingNo = user.Address.BuildingNo,
                    Floor = user.Address.Floor
                };

                return View(profileVM);
            }

            // POST: /Profile/Edit
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(ProfileVM model)
            {
            var userId = int.Parse(_userManager.GetUserId(User));

            var user = await _context.Users
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return RedirectToAction("Login", "Auth");

            // لو الـ Address فاضي، نعمله جديد
            if (user.Address == null)
            {
                user.Address = new Address();
            }

            // تحديث البيانات
            user.Name = model.Name;
            user.PhoneNumber = model.PhoneNumber;
            user.Address.City = model.City;
            user.Address.Street = model.Street;
            user.Address.BuildingNo = model.BuildingNo;
            user.Address.Floor = model.Floor;

            // تحديث الـ DB
            _context.Update(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Profile updated successfully!";
            return RedirectToAction("Index");

        }
    }
    }

