namespace CapitalGainCalculator.Common.Models
{
    public record Transaction
    {
        public Transaction(TransactionType transactionType, Asset asset, DateTimeOffset transactionDate, decimal numberOfShares, decimal unitPrice, decimal transactionCosts)
        {
            TransactionDate = transactionDate;
            Asset = asset ?? throw new ArgumentException("Transaction must be associated with an asset", "asset");
            UnitPrice = unitPrice;
            NumberOfShares = NormaliseNumberOfShares(numberOfShares, transactionType);
            TransactionCosts = transactionCosts;
            TransactionType = transactionType;
        }
        public Asset Asset {get; init;}
        public decimal UnitPrice {get; init;}
        public decimal NumberOfShares {get; init;}
        public decimal TransactionCosts {get; init;}
        public DateTimeOffset TransactionDate {get; init;}
        public TransactionType TransactionType {get; init;}

        private decimal NormaliseNumberOfShares(decimal numberOfShares, TransactionType transactionType) => transactionType switch
        {
            TransactionType.Disposal when numberOfShares > 0 => numberOfShares * -1,
            TransactionType.Purchase when numberOfShares < 0 => numberOfShares * -1,
            _ => numberOfShares
        };
    }
}