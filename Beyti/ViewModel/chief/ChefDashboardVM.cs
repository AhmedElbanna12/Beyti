namespace Beyti.ViewModel.chief
{
    public class ChefDashboardVM
    {
        public string ChefName { get; set; } = null!;
        public int WorkingHours { get; set; }
        public decimal DeliveryFeePerKm { get; set; }

        public List<RecipeVM> Recipes { get; set; } = new();
        public List<OrderVM> Orders { get; set; } = new();
        public List<SupplyVM> Supplies { get; set; } = new();

        public decimal WalletBalance { get; set; }
        public List<WalletTransactionVM> Transactions { get; set; } = new();
    }
}
