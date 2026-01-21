using Beyti.Models;

namespace Beyti.ViewModel.chief
{
    public class OrderVM
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = "";
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "";
        public DateTime OrderTime { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public List<OrderDetailVM> Details { get; set; } = new List<OrderDetailVM>();
    }
}
