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
            var cumulativeCost = accumulator.Cost - (accumulator.Cost * invertedNoOfShares / accumulator.NumberOfShares);
            var cumulativeShares = accumulator.NumberOfShares - invertedNoOfShares;
            return new CumulativeGainData 
            {
                Cost = cumulativeCost,
                NumberOfShares = cumulativeShares
            };
        }
    }
}