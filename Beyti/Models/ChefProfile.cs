namespace Beyti.Models
{
    public class ChefProfile 
    {
        public int Id { get; set; }
        public int WorkingHours { get; set; }
        public decimal DeliveryFeePerKm { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

        public ICollection<SupplierChef> SupplierChefs { get; set; } = new List<SupplierChef>();

    }
}
