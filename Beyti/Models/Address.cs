namespace Beyti.Models
{
    public class Address
    {

        public int Id { get; set; }
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string BuildingNo { get; set; } = null!;
        public string Floor { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
