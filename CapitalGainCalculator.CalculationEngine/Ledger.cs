using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;

namespace CapitalGainCalculator.CalculationEngine
{
    public class Ledger : ILedger
    {
        private readonly ICollection<Transaction> _transactions;
        public Ledger()
        {
            _transactions = new List<Transaction>();
        }

        public CumulativeGainData GetCumulativeGainData(Asset asset, DateTimeOffset? atTimePoint = null)
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

        public void RegisterTransaction(Transaction transaction) => _transactions.Add(transaction ?? throw new ArgumentException("No transaction provided"));
        
        public IEnumerable<Transaction> GetTransactionsByAsset(Asset asset) => 
            _transactions
            .Where(t => t.Asset.Name == asset.Name)
            .OrderBy(t => t.TransactionDate);

        public IEnumerable<Asset> Assets => 
            _transactions
            .Select(t => t.Asset)
            .Distinct();
    }
}