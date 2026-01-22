using Beyti.Data;
using Beyti.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Beyti.Controllers.Delivery
{
    [Authorize(Roles = "Delivery")]
    public class DeliveryOrdersController : Controller
    {
        private readonly BeytiDbContext _context;

        public DeliveryOrdersController(BeytiDbContext context)
        {
            _context = context;
        }

        // Available Orders to Accept
        public async Task<IActionResult> AvailableOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.Customer)
                    .ThenInclude(c => c.Address)
                .Include(o => o.Chef)
                .Include(o => o.OrderDetails)
                    .ThenInclude(d => d.Recipe)
                .Where(o => o.Status == OrderStatus.WaitingForDelivery)
                .ToListAsync();

            return View(orders);
        }

        // Accept an order for delivery
        [HttpPost]
        public async Task<IActionResult> Accept(int id)
        {
            var deliveryId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var order = await _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.Id == id && o.Status == OrderStatus.WaitingForDelivery);

            if (order == null) return NotFound();

            order.DeliveryId = deliveryId;
            order.Status = OrderStatus.AcceptedByDelivery;
            order.AssignedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(InProgressOrders));
        }

        // Orders in progress (accepted by delivery)
        public async Task<IActionResult> InProgressOrders()
        {
            var deliveryId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var orders = await _context.Orders
                .Include(o => o.Customer)
                    .ThenInclude(c => c.Address)
                .Include(o => o.Chef)
                .Include(o => o.OrderDetails)
                    .ThenInclude(d => d.Recipe)
                .Where(o => o.DeliveryId == deliveryId &&
                            (o.Status == OrderStatus.AcceptedByDelivery || o.Status == OrderStatus.OnTheWay))
                .ToListAsync();

            return View(orders);
        }

        // Start delivery (OnTheWay)
        [HttpPost]
        public async Task<IActionResult> StartDelivery(int id)
        {
            var deliveryId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id && o.DeliveryId == deliveryId && o.Status == OrderStatus.AcceptedByDelivery);

            if (order == null) return NotFound();

            order.Status = OrderStatus.OnTheWay;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(InProgressOrders));
        }

        // Mark as Delivered
        [HttpPost]
        public async Task<IActionResult> Delivered(int id)
        {
            var deliveryId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id && o.DeliveryId == deliveryId && o.Status == OrderStatus.OnTheWay);

            if (order == null) return NotFound();

            order.Status = OrderStatus.Delivered;
            order.DeliveryTime = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CompletedOrders));
        }

        // Completed deliveries
        public async Task<IActionResult> CompletedOrders()
        {
            var deliveryId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var orders = await _context.Orders
                .Include(o => o.Customer)
                    .ThenInclude(c => c.Address)
                .Include(o => o.Chef)
                .Include(o => o.OrderDetails)
                    .ThenInclude(d => d.Recipe)
                .Where(o => o.DeliveryId == deliveryId && o.Status == OrderStatus.Delivered)
                .ToListAsync();

            return View(orders);
        }
    }

}

