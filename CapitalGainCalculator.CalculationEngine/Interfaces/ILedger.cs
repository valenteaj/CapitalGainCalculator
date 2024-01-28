using CapitalGainCalculator.CalculationEngine.Models;

namespace CapitalGainCalculator.CalculationEngine.Interfaces
{
    public interface ILedger
    {
        public void RegisterTransaction(Transaction transaction);
        public CumulativeGainData GetCumulativeGainData(IAsset asset, DateTimeOffset? atTimePoint = null);
        public IEnumerable<IAsset> Assets {get;}
        public IEnumerable<Transaction> GetTransactionsByAsset(IAsset asset);
    }
}