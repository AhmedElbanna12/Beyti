namespace Beyti.Models
{
    public class SupplierProfile 
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = null!;
        public string SupplyCategory { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public ICollection<Supply> Supplies { get; set; } = new List<Supply>();

        public ICollection<SupplierChef> SupplierChefs { get; set; } = new List<SupplierChef>();

    }
}
