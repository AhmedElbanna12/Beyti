namespace Beyti.Models
{
    public class OrderDetail
    {

        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
