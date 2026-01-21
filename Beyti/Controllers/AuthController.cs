using Beyti.Models;
using Beyti.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Beyti.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET
        public IActionResult Register() => View();
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Name = model.Name,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            // ✅ Assign Role
            await _userManager.AddToRoleAsync(user, model.Role);

            // ✅ Address
            var address = new Address
            {
                City = model.City,
                Street = model.Street,
                BuildingNo = model.BuildingNo,
                Floor = model.Floor,
                UserId = user.Id
            };

            // ✅ Wallet
            var wallet = new Wallet
            {
                UserId = user.Id,
                Balance = 0,
                IsActive = true
            };

            using var scope = HttpContext.RequestServices.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<Beyti.Data.BeytiDbContext>();

            db.Addresses.Add(address);
            db.Wallets.Add(wallet);

            // ✅ Create Profile حسب الدور
            switch (model.Role)
            {
                case "Customer":
                    db.CustomerProfiles.Add(new CustomerProfile { UserId = user.Id });
                    break;

                case "Chef":
                    db.ChefProfiles.Add(new ChefProfile
                    {
                        UserId = user.Id,
                        WorkingHours = 8,
                        DeliveryFeePerKm = 0
                    });
                    break;

                case "Supplier":
                    db.SupplierProfiles.Add(new SupplierProfile
                    {
                        UserId = user.Id,
                        CompanyName = "New Supplier",
                        SupplyCategory = "General"
                    });
                    break;

                case "Delivery":
                    db.DeliveryProfiles.Add(new DeliveryProfile
                    {
                        UserId = user.Id,
                        TransportType = "Bike",
                        CoveredAreas = "Local"
                    });
                    break;
            }

            await db.SaveChangesAsync();

            await _signInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Home");
        }

        // GET
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(
                model.Email, model.Password, model.RememberMe, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid login attempt");
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Chef"))
                return RedirectToAction("Dashboard", "Chef");

            if (roles.Contains("Supplier"))
                return RedirectToAction("Dashboard", "Supplier");

            if (roles.Contains("Delivery"))
                return RedirectToAction("Dashboard", "Delivery");

            return RedirectToAction("Index", "Home"); // Customer
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

