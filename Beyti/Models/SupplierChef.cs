namespace Beyti.Models
{
    public class SupplierChef
    {
        public int Id { get; set; }

        public int SupplierProfileId { get; set; }
        public SupplierProfile SupplierProfile { get; set; } = null!;

        public int ChefProfileId { get; set; }
        public ChefProfile ChefProfile { get; set; } = null!;

    }
}
