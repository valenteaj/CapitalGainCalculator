using CapitalGainCalculator.CalculationEngine.Models;

namespace CapitalGainCalculator.CalculationEngine.Services;

public class PurchaseService : IPurchaseService
{
    public CumulativeGainData Aggregate(Transaction transaction, CumulativeGainData accumulator)
    {
        accumulator.TotalProofOfActualCost += transaction.UnitPrice * transaction.NumberOfShares + transaction.TransactionCosts;
        accumulator.TotalNumberOfShares += transaction.NumberOfShares;
        return accumulator;
    }
}
