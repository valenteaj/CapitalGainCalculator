using CapitalGainCalculator.Common.Interfaces;

namespace CapitalGainCalculator.Common.Models
{
    public class Purchase : Transaction
    {
        protected override string TransactionType => "Purchase";

        public Purchase(IAsset asset, decimal unitPrice, decimal numberOfShares, decimal transactionCosts) 
            : base(asset, unitPrice, numberOfShares, transactionCosts)
        {
        }
        
        public override decimal ProofOfActualCost => (UnitPrice * NumberOfShares) + TransactionCosts;
    }
}