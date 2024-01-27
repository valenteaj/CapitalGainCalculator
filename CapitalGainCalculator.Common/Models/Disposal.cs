using CapitalGainCalculator.Common.Interfaces;

namespace CapitalGainCalculator.Common.Models
{
    public class Disposal : Transaction
    {
        protected override string TransactionType => "Disposal";

        public Disposal(IAsset asset, DateTimeOffset purchaseDate, decimal unitPrice, decimal numberOfShares, decimal transactionCosts) 
            : base(asset, purchaseDate, unitPrice, -numberOfShares, transactionCosts)
        {
        }

        public override CumulativeGainData Aggregate(CumulativeGainData accumulator)
        {
            var invertedNoOfShares = NumberOfShares * -1;
            accumulator.TotalProofOfActualCost -= accumulator.TotalProofOfActualCost * invertedNoOfShares / accumulator.TotalNumberOfShares;
            accumulator.TotalNumberOfShares -= invertedNoOfShares;
            return accumulator;
        }
    }
}