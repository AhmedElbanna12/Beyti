namespace Beyti.ViewModel.chief
{
    public class WalletTransactionVM
    {
        public DateTime CreatedAt { get; set; }
        public decimal Amount { get; set; }
        public string Direction { get; set; } = "";
        public string Type { get; set; } = "";
    }
}
