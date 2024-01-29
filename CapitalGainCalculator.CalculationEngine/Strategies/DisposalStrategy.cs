using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;

namespace CapitalGainCalculator.CalculationEngine.Strategies
{
    public class DisposalStrategy : ITransactionStrategy
    {
        public TransactionType TransactionType => TransactionType.Disposal;

        public CumulativeGainData Aggregate(Transaction candidate, CumulativeGainData accumulator)
        {
            var invertedNoOfShares = candidate.NumberOfShares * -1;
            accumulator.TotalProofOfActualCost -= accumulator.TotalProofOfActualCost * invertedNoOfShares / accumulator.TotalNumberOfShares;
            accumulator.TotalNumberOfShares -= invertedNoOfShares;
            return accumulator;
        }
    }
}