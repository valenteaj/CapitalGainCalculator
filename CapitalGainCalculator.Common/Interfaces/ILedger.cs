using CapitalGainCalculator.Common.Models;

namespace CapitalGainCalculator.Common.Interfaces
{
    public interface ILedger
    {
        public void RegisterTransaction(Transaction transaction);
        public decimal TotalProofOfActualCost(IAsset asset);
        public decimal TotalNumberOfShares(IAsset asset);
    }
}