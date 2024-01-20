using CapitalGainCalculator.Common.Interfaces;

namespace CapitalGainCalculator.Common.Models
{
    public class Asset : IAsset
    {
        private readonly ILedger _ledger;
        public string Name {get;init;}
        private Asset(string name, ILedger ledger)
        {
            Name = name;
            _ledger = ledger;
        }
        public static IAsset CreateObject(string name) => new Asset(name, Ledger.CreateObject());

        public decimal TotalProofOfActualCost => _ledger.TotalProofOfActualCost(this);

        public void Buy(decimal quantity, decimal unitPrice, decimal transactionCosts)
        {
            var transaction = new Purchase(this, unitPrice, quantity, transactionCosts);
            _ledger.RegisterTransaction(transaction);
        }

        public decimal Sell(decimal quantity, decimal currentPrice, decimal transactionCosts) 
        {
            var disposalProceeds = quantity * currentPrice;
            var proofOfActualCost = _ledger.TotalProofOfActualCost(this);
            var totalNumberOfShares = _ledger.TotalNumberOfShares(this);
            var allowableCost = proofOfActualCost * quantity / totalNumberOfShares;
            var chargeableGain = disposalProceeds - allowableCost - transactionCosts;
            
            var transaction = new Disposal(this, currentPrice, quantity, totalNumberOfShares, proofOfActualCost, transactionCosts);
            _ledger.RegisterTransaction(transaction);
            return chargeableGain;
        }
    }
}