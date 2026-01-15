namespace Beyti.Models
{
    public class CustomerProfile
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
