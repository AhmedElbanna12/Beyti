namespace Beyti.Models
{
    public class DeliveryProfile
    {
        public int Id { get; set; }
        public string TransportType { get; set; } = null!;
        public string CoveredAreas { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
