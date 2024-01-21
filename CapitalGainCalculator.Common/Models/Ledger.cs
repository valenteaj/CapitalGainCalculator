using System.Text;
using CapitalGainCalculator.Common.Interfaces;

namespace CapitalGainCalculator.Common.Models
{
    public class Ledger : ILedger
    {
        private readonly IList<Transaction> _transactions;
        public Ledger()
        {
            _transactions = new List<Transaction>();
        }

        public decimal TotalNumberOfShares(IAsset asset) => _transactions.Where(t => t.Asset.Name == asset.Name).Sum(t => t.NumberOfShares);

        public decimal TotalProofOfActualCost(IAsset asset) => _transactions.Where(t => t.Asset.Name == asset.Name).Sum(t => t.ProofOfActualCost);

        public void RegisterTransaction(Transaction transaction)
        {
            _transactions.Add(transaction);
        }

        public override string ToString() 
        {
            var builder = new StringBuilder();
            foreach (var transaction in _transactions)
            {
                builder.AppendLine(transaction.ToString());
            }
            return builder.ToString();
        }
    }
}