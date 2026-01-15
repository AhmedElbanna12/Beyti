namespace Beyti.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Ingredients { get; set; } = null!;
        public int PreparationTime { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; } = null!;

        public int ChefProfileId { get; set; }
        public ChefProfile ChefProfile { get; set; } = null!;
    }
}
