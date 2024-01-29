using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;
using CapitalGainCalculator.CalculationEngine.Strategies;

namespace CapitalGainCalculator.CalculationEngine
{
    public class Ledger : ILedger
    {
        private readonly IStore<Transaction> _transactionStore;
        private readonly TransactionStrategyCoordinator _context;
        public Ledger(IStore<Transaction> transactionStore, IEnumerable<ITransactionStrategy> transactionStrategies)
        {
            _transactionStore = transactionStore;
            _context = new TransactionStrategyCoordinator(transactionStrategies);
        }

        public CumulativeGainData GetCumulativeGainData(Asset asset, DateTimeOffset? atTimePoint = null)
        {
            CumulativeGainData data = new CumulativeGainData();
            
            var assetTransactions = 
                GetTransactionsByAsset(asset)
                .Where(t => t.TransactionDate < (atTimePoint ?? DateTimeOffset.MaxValue));

            foreach (var transaction in assetTransactions)
            {
                _context.SetStrategy(transaction.TransactionType);
                data = _context.ExecuteAggregate(transaction, data);
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