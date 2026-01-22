using Beyti.Data;
using Beyti.Models;
using Beyti.ViewModel.Delivery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Beyti.Controllers.Delivery
{

    [Authorize(Roles = "Delivery")]
    public class DeliveryDashboardController : Controller
    {
        private readonly BeytiDbContext _context;

        public DeliveryDashboardController(BeytiDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var driverId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var waitingCount = await _context.Orders
                .CountAsync(o => o.Status == OrderStatus.WaitingForDelivery);

            var inProgressCount = await _context.Orders
                .CountAsync(o => o.DeliveryId == driverId &&
                                (o.Status == OrderStatus.AcceptedByDelivery || o.Status == OrderStatus.OnTheWay));

            var completedCount = await _context.Orders
                .CountAsync(o => o.DeliveryId == driverId && o.Status == OrderStatus.Delivered);

            var vm = new DeliveryDashboardVM
            {
                WaitingOrdersCount = waitingCount,
                InProgressOrdersCount = inProgressCount,
                CompletedOrdersCount = completedCount
            };

            return View(vm);
        }
    }
}
