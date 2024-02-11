using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;

namespace CapitalGainCalculator.CalculationEngine.Filters
{
    public class PrecedingTransactionsFilter : ITransactionFilter
    {
        public IEnumerable<Transaction> Filter(Transaction filterCandidate, IEnumerable<Transaction> transactions)
        {
            return transactions.Where(t => t.TransactionDate < filterCandidate.TransactionDate).OrderBy(t => t.TransactionDate);
        }
    }
}