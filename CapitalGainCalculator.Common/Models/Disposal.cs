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
    }
}