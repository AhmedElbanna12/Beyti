namespace Beyti.ViewModel.Customer
{
    public class CustomerRecipeVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string Ingredients { get; set; } = null!;
        public int PreparationTime { get; set; }
        public string ImagePath { get; set; } = null!;
        

    }
}
