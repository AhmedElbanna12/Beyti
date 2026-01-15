namespace Beyti.Models
{
    public class Wallet
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public decimal Balance { get; set; }
        public bool IsActive { get; set; }

        public ICollection<WalletTransaction> Transactions { get; set; } = new List<WalletTransaction>();
    }
}
