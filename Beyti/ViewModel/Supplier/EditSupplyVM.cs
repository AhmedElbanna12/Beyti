namespace Beyti.ViewModel.Supplier
{
    public class EditSupplyVM
    {
        public int Id { get; set; }  // مهم لتحديد الـ supply اللي هيتعدل
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string QualityLevel { get; set; } = null!;
    }
}
