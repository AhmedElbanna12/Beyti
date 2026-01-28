using Beyti.Data;
using Beyti.Dtos;
using Beyti.Models;
using Beyti.ViewModel.chief;
using Beyti.ViewModel.Customer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Beyti.Controllers.Customer
{
    [Authorize(Roles = "Customer")]
    public class CustomerOrdersController : Controller
    {
        private readonly BeytiDbContext _context;

        private readonly UserManager<User> _userManager;

        public CustomerOrdersController(BeytiDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Chefs()
        {
            var users = await _userManager.GetUsersInRoleAsync("Chef");
            var chefs = await _context.ChefProfiles
                .Include(cp => cp.User)
                .Where(cp => users.Select(u => u.Id).Contains(cp.UserId))
                .ToListAsync();

            return View(chefs);
        }


        // GET: Chef Menu
        public async Task<IActionResult> ChefMenu(int chefId)
        {
            var chefProfile = await _context.ChefProfiles
                .Include(cp => cp.Recipes)
                .Include(cp => cp.User)
                .FirstOrDefaultAsync(cp => cp.UserId == chefId);

            if (chefProfile == null)
                return NotFound();

            var vm = chefProfile.Recipes.Select(r => new CustomerRecipeVM
            {
                Id = r.Id,
                Title = r.Title,
                Category = r.Category,
                Description = r.Description,
                Ingredients = r.Ingredients,          // ✅ أهو
                PreparationTime = r.PreparationTime,  // ✅ وأهو
                Price = r.Price,
                ImagePath = r.Image
            }).ToList();

            ViewBag.ChefName = chefProfile.User.Name;
            ViewBag.ChefId = chefId;

            return View(vm);
        }


        // POST: Place order
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(PlaceOrderDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var order = new Order
            {
                CustomerId = userId,
                ChefId = dto.ChefId,
                Status = OrderStatus.Pending,
                OrderTime = DateTime.UtcNow,
                TotalPrice = 0m,
                OrderDetails = new List<OrderDetail>()
            };

            foreach (var item in dto.Items)
            {
                var recipe = await _context.Recipes.FindAsync(item.RecipeId);
                if (recipe != null && item.Quantity > 0)
                {
                    order.OrderDetails.Add(new OrderDetail
                    {
                        RecipeId = recipe.Id,
                        Quantity = item.Quantity,
                        Price = recipe.Price
                    });
                    order.TotalPrice += recipe.Price * item.Quantity;
                }
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("MyOrders");
        }

        // GET: My Orders
        public async Task<IActionResult> MyOrders()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var orders = await _context.Orders
                .Where(o => o.CustomerId == userId)
                .Include(o => o.Chef)
                .Include(o => o.OrderDetails)
                    .ThenInclude(d => d.Recipe)
                .OrderByDescending(o => o.OrderTime)
                .ToListAsync();

            var vm = orders.Select(o => new CustomerOrderVM
            {
                Id = o.Id,
                ChefName = o.Chef.Name,
                TotalPrice = o.TotalPrice,
                OrderTime = o.OrderTime,
                Status = o.Status,
                Details = o.OrderDetails.Select(d => new CustomerOrderDetailVM
                {
                    RecipeTitle = d.Recipe.Title,
                    Quantity = d.Quantity,
                    Price = d.Price
                }).ToList()
            }).ToList();

            return View(vm);
        }

        // POST: Cancel Order
        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id && o.CustomerId == userId);

            if (order == null || order.Status != OrderStatus.Pending)
                return BadRequest();

            order.Status = OrderStatus.Cancelled;
            await _context.SaveChangesAsync();

            return RedirectToAction("MyOrders");
        }
    }
}
