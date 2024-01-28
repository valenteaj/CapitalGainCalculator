namespace CapitalGainCalculator.CalculationEngine.Models
{
    public class Purchase : Transaction
    {
        protected override TransactionType TransactionType => TransactionType.Purchase;

        public Purchase(Asset asset, DateTimeOffset purchaseDate, decimal unitPrice, decimal numberOfShares, decimal transactionCosts) 
            : base(asset, purchaseDate, unitPrice, numberOfShares, transactionCosts)
        {
        }

        public override CumulativeGainData Aggregate(CumulativeGainData accumulator)
        {
            accumulator.TotalProofOfActualCost += UnitPrice * NumberOfShares + TransactionCosts;
            accumulator.TotalNumberOfShares += NumberOfShares;
            return accumulator;
        }
    }
}