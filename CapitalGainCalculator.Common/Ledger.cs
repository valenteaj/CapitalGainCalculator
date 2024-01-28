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

        public CumulativeGainData GetCumulativeGainData(IAsset asset, DateTimeOffset? atTimePoint = null)
        {
            CumulativeGainData data = new CumulativeGainData();
            
            var assetTransactions = 
                GetTransactionsByAsset(asset)
                .Where(t => t.TransactionDate < (atTimePoint ?? DateTimeOffset.MaxValue));

            foreach (var transaction in assetTransactions)
            {
                data = transaction.Aggregate(data);
            }
            return data;
        } 

        public void RegisterTransaction(Transaction transaction) => _transactions.Add(transaction);
        
        public IEnumerable<Transaction> GetTransactionsByAsset(IAsset asset) => 
            _transactions
            .Where(t => t.Asset.Name == asset.Name)
            .OrderBy(t => t.TransactionDate);

        public IEnumerable<IAsset> Assets => 
            _transactions
            .Select(t => t.Asset)
            .Distinct();
        
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