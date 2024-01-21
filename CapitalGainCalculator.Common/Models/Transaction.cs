using CapitalGainCalculator.Common.Interfaces;

namespace CapitalGainCalculator.Common.Models
{
    public abstract class Transaction
    {
        public Transaction(IAsset asset, DateTimeOffset transactionDate, decimal unitPrice, decimal numberOfShares, decimal transactionCosts)
        {
            TransactionDate = transactionDate;
            Asset = asset;
            UnitPrice = unitPrice;
            NumberOfShares = numberOfShares;
            TransactionCosts = transactionCosts;
        }
        public IAsset Asset {get; init;}
        public decimal UnitPrice {get; init;}
        public decimal NumberOfShares {get; init;}
        public decimal TransactionCosts {get; init;}
        public abstract decimal ProofOfActualCost {get;}
        public DateTimeOffset TransactionDate {get; init;}
        protected abstract string TransactionType {get;}
        public override string ToString()
        {
            return $"{TransactionDate} {TransactionType} [{Asset.Name}]: {NumberOfShares} @ {UnitPrice:C2}/share";
        }
    }
}