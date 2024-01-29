using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;

namespace CapitalGainCalculator.CalculationEngine.Strategies
{
    public class PurchaseStrategy : ITransactionStrategy
    {
        public TransactionType TransactionType => TransactionType.Purchase;

        public CumulativeGainData Aggregate(Transaction candidate, CumulativeGainData accumulator)
        {
            accumulator.TotalProofOfActualCost += candidate.UnitPrice * candidate.NumberOfShares + candidate.TransactionCosts;
            accumulator.TotalNumberOfShares += candidate.NumberOfShares;
            return accumulator;
        }
    }
}