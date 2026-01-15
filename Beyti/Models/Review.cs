namespace Beyti.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int FromCustomerId { get; set; }
        public CustomerProfile Customer { get; set; } = null!;

        public int ToUserId { get; set; }
        public User ToUser { get; set; } = null!;

        public int Rating { get; set; }
        public string Comment { get; set; } = null!;
        public DateTime ReviewDate { get; set; }
    }
}
