using CapitalGainCalculator.CalculationEngine.Models;

namespace CapitalGainCalculator.CalculationEngine.Interfaces
{
    public interface ITransactionStrategy
    {
        public TransactionType TransactionType { get; }
        public CumulativeGainData Aggregate(Transaction candidate, CumulativeGainData accumulator);
    }
}