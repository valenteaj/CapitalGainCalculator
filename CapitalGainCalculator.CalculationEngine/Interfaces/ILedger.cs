using CapitalGainCalculator.CalculationEngine.Models;

namespace CapitalGainCalculator.CalculationEngine.Interfaces
{
    public interface ILedger
    {
        public void RegisterTransaction(Transaction transaction);
        public CumulativeGainData GetCumulativeGainData(Asset asset, DateTimeOffset? atTimePoint = null);
        public IEnumerable<Asset> Assets {get;}
        public IEnumerable<Transaction> GetTransactionsByAsset(Asset asset);
    }
}