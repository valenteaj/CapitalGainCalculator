using CapitalGainCalculator.Common.Interfaces;
using CapitalGainCalculator.Common.Models;

namespace CapitalGainCalculator.Common
{
    public class AssetManager : IAssetManager
    {
        private readonly ILedger _ledger;
        public AssetManager(ILedger ledger)
        {
            _ledger = ledger;
        }

        public Purchase Buy(IAsset asset, decimal quantity, decimal unitPrice, decimal transactionCosts, DateTimeOffset? transactionDate = null)
        {
            var transaction = new Purchase(asset, transactionDate ?? DateTimeOffset.UtcNow, unitPrice, quantity, transactionCosts);
            _ledger.RegisterTransaction(transaction);
            return transaction;
        }

        public Disposal Sell(IAsset asset, decimal quantity, decimal currentPrice, decimal transactionCosts, DateTimeOffset? transactionDate = null)
        {
            var transaction = new Disposal(asset, transactionDate ?? DateTimeOffset.UtcNow, currentPrice, quantity, transactionCosts);
            _ledger.RegisterTransaction(transaction);
            return transaction;
        }

        public decimal CalculateChargeableGain(Disposal disposal)
        {
            var disposalProceeds = disposal.NumberOfShares * disposal.UnitPrice;
            var totalNumberOfShares = _ledger.TotalNumberOfShares(disposal.Asset, disposal.TransactionDate);
            var allowableCost = _ledger.TotalProofOfActualCost(disposal.Asset, disposal.TransactionDate) * disposal.NumberOfShares / totalNumberOfShares;
            
            var chargeableGain = ((disposalProceeds - allowableCost) * -1) - disposal.TransactionCosts;
            return chargeableGain;
        }
    }
}