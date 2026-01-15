namespace Beyti.Models
{
    public class Supply
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string QualityLevel { get; set; } = null!;
        public string Image { get; set; } = null!;

        public int SupplierProfileId { get; set; }
        public SupplierProfile SupplierProfile { get; set; } = null!;

    }
}
