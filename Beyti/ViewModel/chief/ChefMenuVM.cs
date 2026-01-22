namespace Beyti.ViewModel.chief
{
    public class ChefMenuVM
    {
        public int ChefId { get; set; }
        public string ChefName { get; set; } = null!;
        public List<RecipeVM> Recipes { get; set; } = new();
    }
}
