using Beyti.Data;
using Beyti.Models;
using Beyti.ViewModel.chief;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Beyti.Controllers.Chef
{


    [Authorize(Roles = "Chef")]
    public class ChefDashboardController : Controller
    {
        private readonly BeytiDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _env;


        public ChefDashboardController(UserManager<User> userManager, BeytiDbContext context , IWebHostEnvironment env)
        {
            _userManager = userManager;
            _context = context;
            _env = env;

        }

        // GET: /ChefDashboard/
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(_userManager.GetUserId(User));

            var chefProfile = await _context.ChefProfiles
                .Include(c => c.User)
                .Include(c => c.Recipes)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (chefProfile == null)
                return RedirectToAction("CreateProfile", "ChefDashboard");

            // Profile Section
            var profileVM = new ChefProfileVM
            {
                Name = chefProfile.User.Name,
                WorkingHours = chefProfile.WorkingHours,
                DeliveryFeePerKm = chefProfile.DeliveryFeePerKm,
                ProfileImage = "", // لو فيه صورة
            };

            // Recipes Section
            var recipesVM = chefProfile.Recipes.Select(r => new RecipeVM
            {
                Id = r.Id,
                Title = r.Title,
                Category = r.Category,
                PreparationTime = r.PreparationTime,
                Price = r.Price,
                ImagePath =r.Image 
            }).ToList();

            // Orders Section
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Recipe)
                .Where(o => o.ChefId == userId)
                .ToListAsync();

            var ordersVM = orders.Select(o => new OrderVM
            {
                Id = o.Id,
                CustomerName = o.Customer.Name,
                TotalPrice = o.TotalPrice,
                Status = o.Status,
                OrderTime = o.OrderTime,
                DeliveryTime = o.DeliveryTime,
                Details = o.OrderDetails.Select(od => new OrderDetailVM
                {
                    RecipeTitle = od.Recipe.Title,
                    Quantity = od.Quantity,
                    Price = od.Price
                }).ToList()
            }).ToList();

            // Supplies Section
            var suppliesVM = await _context.Supplies
                .Include(s => s.SupplierProfile)
                .Select(s => new SupplyVM
                {
                    SupplierName = s.SupplierProfile.CompanyName,
                    Category = s.Category,
                    QualityLevel = s.QualityLevel,
                }).ToListAsync();

            // Wallet Section
            var wallet = await _context.Wallets
                .Include(w => w.Transactions)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            var walletVM = new WalletVM
            {
                Balance = wallet?.Balance ?? 0,
                Transactions = wallet?.Transactions.Select(t => new WalletTransactionVM
                {
                    CreatedAt = t.CreatedAt,
                    Amount = t.Amount,
                    Direction = t.Direction,
                    Type = t.Type
                }).ToList() ?? new List<WalletTransactionVM>()
            };

            var dashboardVM = new ChefDashboardVM
            {
                ChefName = chefProfile.User.Name,
                WorkingHours = chefProfile.WorkingHours,
                DeliveryFeePerKm = chefProfile.DeliveryFeePerKm,
                Recipes = recipesVM,
                Orders = ordersVM,
                Supplies = suppliesVM,
                WalletBalance = wallet?.Balance ?? 0,
                Transactions = wallet?.Transactions.Select(t => new WalletTransactionVM
                {
                    CreatedAt = t.CreatedAt,
                    Amount = t.Amount,
                    Direction = t.Direction,
                    Type = t.Type
                }).ToList() ?? new List<WalletTransactionVM>()
            };

            return View(dashboardVM);


        }

        // GET: Create recipe
        public IActionResult Create()
        {
            return View(new RecipeVM());
        }

        // POST: Create recipe
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecipeVM vm)
        {

            if (!ModelState.IsValid) return View(vm);

            var userId = int.Parse(_userManager.GetUserId(User));
            var chefProfile = await _context.ChefProfiles
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (chefProfile == null)
            {
                // لو البروفايل مش موجود، ارجع لصفحة انشاء بروفايل
                return RedirectToAction("CreateProfile", "ChefDashboard");
            }



            string? imagePath = null;
            if (vm.ImageFile != null)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(vm.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);
                await vm.ImageFile.CopyToAsync(fileStream);

                imagePath = "/uploads/" + fileName; // path to store in DB
            }

            var recipe = new Recipe

            {

                // استخدم الـ Id الفعلي للـ ChefProfile
                Title = vm.Title,
                Category = vm.Category,
                Description = vm.Description,
                Ingredients = vm.Ingredients,
                PreparationTime = vm.PreparationTime,
                Price = vm.Price,
                ChefProfileId = chefProfile.Id,
                Image = imagePath ?? ""
            };

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = int.Parse(_userManager.GetUserId(User));

            var chefProfile = await _context.ChefProfiles
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (chefProfile == null) return Unauthorized();

            var recipe = await _context.Recipes
                .FirstOrDefaultAsync(r => r.Id == id && r.ChefProfileId == chefProfile.Id);

            if (recipe == null) return NotFound();

            var vm = new RecipeVM
            {
                Id = recipe.Id,
                Title = recipe.Title,
                Category = recipe.Category,
                Description = recipe.Description,
                Ingredients = recipe.Ingredients,
                PreparationTime = recipe.PreparationTime,
                Price = recipe.Price,
                ImagePath = recipe.Image
            };

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RecipeVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var userId = int.Parse(_userManager.GetUserId(User));

            var chefProfile = await _context.ChefProfiles
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (chefProfile == null) return Unauthorized();

            var recipe = await _context.Recipes
                .FirstOrDefaultAsync(r => r.Id == id && r.ChefProfileId == chefProfile.Id);

            if (recipe == null) return NotFound();

            if (vm.ImageFile != null)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                // حذف القديمة
                if (!string.IsNullOrEmpty(recipe.Image))
                {
                    var oldPath = Path.Combine(_env.WebRootPath, recipe.Image.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                var fileName = Guid.NewGuid() + Path.GetExtension(vm.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await vm.ImageFile.CopyToAsync(stream);

                recipe.Image = "/uploads/" + fileName;
            }

            recipe.Title = vm.Title;
            recipe.Category = vm.Category;
            recipe.Description = vm.Description;
            recipe.Ingredients = vm.Ingredients;
            recipe.PreparationTime = vm.PreparationTime;
            recipe.Price = vm.Price;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(_userManager.GetUserId(User));

            var chefProfile = await _context.ChefProfiles
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (chefProfile == null)
                return Unauthorized();

            var recipe = await _context.Recipes
                .FirstOrDefaultAsync(r => r.Id == id && r.ChefProfileId == chefProfile.Id);

            if (recipe == null)
                return NotFound();

            // حذف الصورة من wwwroot
            if (!string.IsNullOrEmpty(recipe.Image))
            {
                var imagePath = Path.Combine(
                    _env.WebRootPath,
                    recipe.Image.TrimStart('/')
                );

                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}

