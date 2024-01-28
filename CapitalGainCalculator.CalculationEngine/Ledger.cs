using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;

namespace CapitalGainCalculator.CalculationEngine
{
    public class Ledger : ILedger
    {
        private readonly IStore<Transaction> _transactionStore;
        public Ledger(IStore<Transaction> transactionStore)
        {
            _transactionStore = transactionStore;
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

        public void RegisterTransaction(Transaction transaction) => _transactionStore.Add(transaction);
        
        public IEnumerable<Transaction> GetTransactionsByAsset(Asset asset) => 
            _transactionStore.Get()
            .Where(t => t.Asset.Name == asset.Name)
            .OrderBy(t => t.TransactionDate);

        public IEnumerable<Asset> Assets => 
            _transactionStore.Get()
            .Select(t => t.Asset)
            .Distinct();
    }
}