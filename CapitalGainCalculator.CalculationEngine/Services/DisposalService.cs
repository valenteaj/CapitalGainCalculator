using CapitalGainCalculator.CalculationEngine.Models;

namespace CapitalGainCalculator.CalculationEngine.Services;

public class DisposalService : IDisposalService
{
    public CumulativeGainData Aggregate(Transaction transaction, CumulativeGainData accumulator)
    {
        var invertedNoOfShares = transaction.NumberOfShares * -1;
        accumulator.TotalProofOfActualCost -= accumulator.TotalProofOfActualCost * invertedNoOfShares / accumulator.TotalNumberOfShares;
        accumulator.TotalNumberOfShares -= invertedNoOfShares;
        return accumulator;
    }
}
