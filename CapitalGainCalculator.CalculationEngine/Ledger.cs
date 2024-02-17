using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;
using CapitalGainCalculator.CalculationEngine.Strategies;

namespace CapitalGainCalculator.CalculationEngine
{
    public class Ledger : ILedger
    {
        private readonly IStore<Transaction> _transactionStore;
        private readonly IEnumerable<ITransactionProcessor> _transactionProcessors;
        private readonly TransactionStrategyCoordinator _context;
        public Ledger(
            IStore<Transaction> transactionStore, 
            IEnumerable<ITransactionStrategy> transactionStrategies,
            IEnumerable<ITransactionProcessor> transactionProcessors)
        {
            _transactionStore = transactionStore;
            _transactionProcessors = transactionProcessors ?? Array.Empty<ITransactionProcessor>();
            _context = new TransactionStrategyCoordinator(transactionStrategies);
        }

        public CumulativeGainData GetCumulativeGainData(Transaction transaction)
        {
            CumulativeGainData data = new CumulativeGainData();
            
            var assetTransactions = 
                GetTransactionsByAsset(transaction.Asset)
                .Where(t => t.TransactionDate < transaction.TransactionDate);

            var postProcessedTransactions = new List<Transaction>(assetTransactions)
            {
                transaction
            };
            
            foreach (var filter in _transactionProcessors)
            {
                postProcessedTransactions = filter.Process(postProcessedTransactions).ToList();
            }

            foreach (var t in postProcessedTransactions.SkipLast(1)) // Skiplast removes the final disposal
            {
                _context.SetStrategy(t.TransactionType);
                data = _context.ExecuteAggregate(t, data);
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