using System.Text;
using CapitalGainCalculator.Common.Interfaces;
using CapitalGainCalculator.Common.Models;

namespace CapitalGainCalculator.Common
{
    public class Ledger : ILedger
    {
        private readonly ICollection<Transaction> _transactions;
        public Ledger()
        {
            _transactions = new List<Transaction>();
        }

        public decimal TotalNumberOfShares(IAsset asset, DateTimeOffset? atTimePoint = null) => _transactions
            .Where(t => t.TransactionDate < (atTimePoint ?? DateTimeOffset.MaxValue))
            .Where(t => t.Asset.Name == asset.Name)
            .Sum(t => t.NumberOfShares);

        public decimal TotalProofOfActualCost(IAsset asset, DateTimeOffset? atTimePoint = null)
        {
            decimal cumulativeShares = 0;
            decimal cumulativeCost = 0;
            
            var assetTransactions = _transactions.Where(t => t.TransactionDate < (atTimePoint ?? DateTimeOffset.MaxValue))
                .Where(t => t.Asset.Name == asset.Name)
                .Where(t => t.TransactionDate < (atTimePoint ?? DateTimeOffset.MaxValue));

            foreach (var transaction in assetTransactions)
            {
                if (transaction is Purchase)
                {
                    cumulativeCost += transaction.UnitPrice * transaction.NumberOfShares + transaction.TransactionCosts;
                    cumulativeShares += transaction.NumberOfShares;
                }
                else 
                {
                    var invertedNoOfShares = transaction.NumberOfShares * -1;
                    cumulativeCost -= cumulativeCost * invertedNoOfShares / cumulativeShares;
                    cumulativeShares -= invertedNoOfShares;
                }
            }
            return cumulativeCost;
        } 

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