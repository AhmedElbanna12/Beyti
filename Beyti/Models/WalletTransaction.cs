namespace Beyti.Models
{
    public class WalletTransaction
    {
        public int Id { get; set; }

        public int WalletId { get; set; }
        public Wallet Wallet { get; set; } = null!;

        public decimal Amount { get; set; }
        public string Direction { get; set; } = null!; // In / Out
        public string Type { get; set; } = null!; // Order / Refund / Commission
        public DateTime CreatedAt { get; set; }
    }
}
