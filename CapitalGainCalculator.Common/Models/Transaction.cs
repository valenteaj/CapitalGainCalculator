using CapitalGainCalculator.Common.Interfaces;

namespace CapitalGainCalculator.Common.Models
{
    public abstract class Transaction
    {
        public Transaction(IAsset asset,decimal unitPrice, decimal numberOfShares, decimal transactionCosts)
        {
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
    }
}