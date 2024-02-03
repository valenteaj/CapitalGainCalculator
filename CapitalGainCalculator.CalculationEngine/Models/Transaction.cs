namespace CapitalGainCalculator.CalculationEngine.Models
{
    public record Transaction
    {
        public Transaction(TransactionType transactionType, Asset asset, DateTimeOffset transactionDate, decimal unitPrice, decimal numberOfShares, decimal transactionCosts)
        {
            TransactionDate = transactionDate;
            Asset = asset ?? throw new ArgumentException("Transaction must be associated with an asset", "asset");
            UnitPrice = unitPrice;
            NumberOfShares = transactionType == TransactionType.Disposal && numberOfShares > 0 ? numberOfShares * -1 : numberOfShares;;
            TransactionCosts = transactionCosts;
            TransactionType = transactionType;
        }
        public Asset Asset {get; init;}
        public decimal UnitPrice {get; init;}
        public decimal NumberOfShares {get; init;}
        public decimal TransactionCosts {get; init;}
        public DateTimeOffset TransactionDate {get; init;}
        public TransactionType TransactionType {get; init;}

        public override string ToString()
        {
            return $"{TransactionDate} {TransactionType} [{Asset.Name}]: {NumberOfShares}@{UnitPrice:C2} = {NumberOfShares*UnitPrice:C2}";
        }
    }
}