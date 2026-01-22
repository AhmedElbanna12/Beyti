using Beyti.Models;

namespace Beyti.ViewModel.Customer
{
    public class CustomerOrderVM
    {
        public int Id { get; set; }
        public string ChefName { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public DateTime OrderTime { get; set; }
        public OrderStatus Status { get; set; }
        public List<CustomerOrderDetailVM> Details { get; set; } = new();
    }
}
