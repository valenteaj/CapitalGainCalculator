using CapitalGainCalculator.CalculationEngine.Interfaces;

namespace CapitalGainCalculator.CalculationEngine.Models
{
    public abstract class Transaction : IAggregate<CumulativeGainData>
    {
        public Transaction(Asset asset, DateTimeOffset transactionDate, decimal unitPrice, decimal numberOfShares, decimal transactionCosts)
        {
            TransactionDate = transactionDate;
            Asset = asset;
            UnitPrice = unitPrice;
            NumberOfShares = numberOfShares;
            TransactionCosts = transactionCosts;
        }
        public Asset Asset {get; init;}
        public decimal UnitPrice {get; init;}
        public decimal NumberOfShares {get; init;}
        public decimal TransactionCosts {get; init;}
        public DateTimeOffset TransactionDate {get; init;}
        protected abstract TransactionType TransactionType {get;}
        public abstract CumulativeGainData Aggregate(CumulativeGainData accumulator);

        public override string ToString()
        {
            return $"{TransactionDate} {TransactionType} [{Asset.Name}]: {NumberOfShares}@{UnitPrice:C2} = {NumberOfShares*UnitPrice:C2}";
        }
    }
}