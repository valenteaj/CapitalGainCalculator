namespace CapitalGainCalculator.CalculationEngine.Models
{
    public class Disposal : Transaction
    {
        protected override TransactionType TransactionType => TransactionType.Disposal;

        public Disposal(Asset asset, DateTimeOffset purchaseDate, decimal unitPrice, decimal numberOfShares, decimal transactionCosts) 
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