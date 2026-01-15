namespace Beyti.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public User Customer { get; set; } = null!;

        public int ChefId { get; set; }
        public User Chef { get; set; } = null!;

        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = null!;
        public DateTime OrderTime { get; set; }
        public DateTime? DeliveryTime { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
