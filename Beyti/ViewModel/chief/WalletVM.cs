namespace Beyti.ViewModel.chief
{
    public class WalletVM
    {
        public decimal Balance { get; set; }
        public List<WalletTransactionVM> Transactions { get; set; } = new List<WalletTransactionVM>();

    }
}
