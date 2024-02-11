using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;

namespace CapitalGainCalculator.CalculationEngine.Filters
{
    public class AssetFilter : ITransactionFilter
    {
        public IEnumerable<Transaction> Filter(Transaction filterCandidate, IEnumerable<Transaction> transactions)
        {
            return transactions.Where(t => t.Asset == filterCandidate.Asset);
        }
    }
}