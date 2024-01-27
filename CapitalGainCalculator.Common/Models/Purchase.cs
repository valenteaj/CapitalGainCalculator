using CapitalGainCalculator.Common.Interfaces;

namespace CapitalGainCalculator.Common.Models
{
    public class Purchase : Transaction
    {
        protected override string TransactionType => "Purchase";

        public Purchase(IAsset asset, DateTimeOffset purchaseDate, decimal unitPrice, decimal numberOfShares, decimal transactionCosts) 
            : base(asset, purchaseDate, unitPrice, numberOfShares, transactionCosts)
        {
        }

        public override CumulativeGainData Aggregate(CumulativeGainData accumulator)
        {
            var cumulativeCost = accumulator.Cost + (UnitPrice * NumberOfShares + TransactionCosts);
            var cumulativeShares = accumulator.NumberOfShares + NumberOfShares;
            return new CumulativeGainData
            {
                Cost = cumulativeCost,
                NumberOfShares = cumulativeShares
            };
        }
    }
}