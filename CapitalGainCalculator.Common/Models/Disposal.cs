using CapitalGainCalculator.Common.Interfaces;

namespace CapitalGainCalculator.Common.Models
{
    public class Disposal : Transaction
    {
        protected override string TransactionType => "Disposal";
        public Disposal(
            IAsset asset,
            decimal unitPrice, 
            decimal numberOfShares, 
            decimal totalNumberOfSharesPriorToDisposal, 
            decimal totalProofOfActualCostPriorToDisposal,
            decimal transactionCosts) 
            : base(asset, unitPrice, -numberOfShares, transactionCosts)
        {
            _totalNumberOfSharePriorToDisposal = totalNumberOfSharesPriorToDisposal;
            _totalProofOfActualCostPriorToDisposal = totalProofOfActualCostPriorToDisposal;
        }
        
        private decimal _totalNumberOfSharePriorToDisposal;
        private decimal _totalProofOfActualCostPriorToDisposal;
        public override decimal ProofOfActualCost => _totalProofOfActualCostPriorToDisposal * NumberOfShares / _totalNumberOfSharePriorToDisposal;
    }
}