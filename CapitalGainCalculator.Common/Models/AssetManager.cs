using CapitalGainCalculator.Common.Interfaces;

namespace CapitalGainCalculator.Common.Models
{
    public class AssetManager : IAssetManager
    {
        private readonly ILedger _ledger;
        public AssetManager(ILedger ledger)
        {
            _ledger = ledger;
        }

        public void Buy(IAsset asset, decimal quantity, decimal unitPrice, decimal transactionCosts)
        {
            var transaction = new Purchase(asset, unitPrice, quantity, transactionCosts);
            _ledger.RegisterTransaction(transaction);
        }

        public decimal Sell(IAsset asset, decimal quantity, decimal currentPrice, decimal transactionCosts)
        {
            var disposalProceeds = quantity * currentPrice;
            var proofOfActualCost = _ledger.TotalProofOfActualCost(asset);
            var totalNumberOfShares = _ledger.TotalNumberOfShares(asset);
            var allowableCost = proofOfActualCost * quantity / totalNumberOfShares;
            var chargeableGain = disposalProceeds - allowableCost - transactionCosts;
            
            var transaction = new Disposal(asset, currentPrice, quantity, totalNumberOfShares, proofOfActualCost, transactionCosts);
            _ledger.RegisterTransaction(transaction);
            return chargeableGain;
        }
    }
}