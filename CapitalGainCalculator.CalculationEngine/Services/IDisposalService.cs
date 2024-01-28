using CapitalGainCalculator.CalculationEngine.Models;

namespace CapitalGainCalculator.CalculationEngine.Services;

public interface IDisposalService
{
    CumulativeGainData Aggregate(Transaction transaction, CumulativeGainData accumulator);
}
