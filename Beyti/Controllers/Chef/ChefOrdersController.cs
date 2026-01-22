using Beyti.Data;
using Beyti.Models;
using Beyti.ViewModel.chief;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Beyti.Controllers.Chef
{

    [Authorize(Roles = "Chef")]
    public class ChefOrdersController : Controller
    {
        private readonly BeytiDbContext _context;

        public ChefOrdersController(BeytiDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var chefId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                    .ThenInclude(d => d.Recipe)
                .Where(o => o.ChefId == chefId)
                .OrderByDescending(o => o.OrderTime)
                .ToListAsync();

            var vm = orders.Select(o => new OrderVM
            {
                Id = o.Id,
                CustomerName = o.Customer.Name,
                TotalPrice = o.TotalPrice,
                Status = o.Status,
                OrderTime = o.OrderTime,
                Details = o.OrderDetails.Select(d => new OrderDetailVM
                {
                    RecipeTitle = d.Recipe.Title,
                    Quantity = d.Quantity,
                    Price = d.Price
                }).ToList()
            }).ToList();

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Accept(int id)
        {
            var chefId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id && o.ChefId == chefId);

            if (order == null || order.Status != OrderStatus.Pending)
                return BadRequest();

            order.Status = OrderStatus.WaitingForDelivery;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var chefId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id && o.ChefId == chefId);

            if (order == null || order.Status != OrderStatus.Pending)
                return BadRequest();

            order.Status = OrderStatus.RejectedByChef;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }

}

