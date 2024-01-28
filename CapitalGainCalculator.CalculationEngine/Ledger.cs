using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;
using CapitalGainCalculator.CalculationEngine.Services;

namespace CapitalGainCalculator.CalculationEngine
{
    public class Ledger : ILedger
    {
        private readonly ITransactionStore _transactionStore;
        private readonly IPurchaseService _purchaseService;
        private readonly IDisposalService _disposalService;

        public Ledger(
            ITransactionStore transactionStore,
            IPurchaseService purchaseService,
            IDisposalService disposalService)
        {
            _transactionStore = transactionStore;
            _purchaseService = purchaseService;
            _disposalService = disposalService;
        }

        public CumulativeGainData GetCumulativeGainData(Asset asset, DateTimeOffset? atTimePoint = null)
        {
            var data = new CumulativeGainData();
            
            var assetTransactions = 
                GetTransactionsByAsset(asset)
                .Where(t => t.TransactionDate < (atTimePoint ?? DateTimeOffset.MaxValue));

            foreach (var transaction in assetTransactions)
            {
                if (transaction.TransactionType == TransactionType.Disposal)
                {
                    data = _disposalService.Aggregate(transaction, data);
                }
                else if (transaction.TransactionType == TransactionType.Purchase)
                {
                    data = _purchaseService.Aggregate(transaction, data);
                }
            }
            return data;
        }

        public void RegisterTransaction(Transaction transaction)
        {
            _transactionStore.Add(transaction);
        }
        
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