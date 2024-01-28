using CapitalGainCalculator.CalculationEngine.Models;

namespace CapitalGainCalculator.CalculationEngine
{
    public interface ITransactionStore
    {
        IEnumerable<Transaction> Get();
        void Add(Transaction transaction);
    }
}