namespace Beyti.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public User Customer { get; set; } = null!;

        public int ChefId { get; set; }
        public User Chef { get; set; } = null!;


        public int? DeliveryId { get; set; }
        public User? Delivery { get; set; }

        public decimal? DeliveryFee { get; set; }
        public DateTime? AssignedAt { get; set; }

        public decimal TotalPrice { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public OrderStatus Status { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    }

    public enum OrderStatus
    {
        Pending,            // الزبون عمل الطلب
        AcceptedByChef,     // الشيف قبل
        RejectedByChef,     // الشيف رفض
        WaitingForDelivery, // مستني موصل
        AcceptedByDelivery, // الموصل قبل
        OnTheWay,           // في الطريق
        Delivered,          // تم التسليم
        Cancelled
    }

}
