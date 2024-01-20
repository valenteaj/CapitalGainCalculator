using CapitalGainCalculator.Common.Interfaces;

namespace CapitalGainCalculator.Common.Models
{
    public class Ledger : ILedger
    {
        private readonly IList<Transaction> _transactions;
        private Ledger()
        {
            _transactions = new List<Transaction>();
        }

        public static ILedger CreateObject() => new Ledger();

        public decimal TotalNumberOfShares(IAsset asset) => _transactions.Where(t => t.Asset.Name == asset.Name).Sum(t => t.NumberOfShares);

        public decimal TotalProofOfActualCost(IAsset asset) => _transactions.Where(t => t.Asset.Name == asset.Name).Sum(t => t.ProofOfActualCost);

        public void RegisterTransaction(Transaction transaction)
        {
            _transactions.Add(transaction);
        }
    }
}