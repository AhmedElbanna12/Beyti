namespace Beyti.ViewModel.chief
{
    public class ChefProfileVM
    {
        public string Name { get; set; } = "Chef";
        public int WorkingHours { get; set; }
        public decimal DeliveryFeePerKm { get; set; }
        public string TransportType { get; set; } = null!;
        public string CoveredAreas { get; set; } = null!;
        public string ProfileImage { get; set; } = "";

    }
}
