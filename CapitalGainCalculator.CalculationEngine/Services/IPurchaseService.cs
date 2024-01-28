using CapitalGainCalculator.CalculationEngine.Models;

namespace CapitalGainCalculator.CalculationEngine.Services;

public interface IPurchaseService
{
    CumulativeGainData Aggregate(Transaction transaction, CumulativeGainData accumulator);
}
