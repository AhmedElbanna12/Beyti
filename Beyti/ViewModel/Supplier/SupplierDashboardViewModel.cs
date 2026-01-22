using Beyti.Models;

namespace Beyti.ViewModel.Supplier
{
    public class SupplierDashboardViewModel
    {
        public SupplierProfile Supplier { get; set; } = null!;
        public List<ChefProfile> AvailableChefs { get; set; } = new List<ChefProfile>();
    }
}
