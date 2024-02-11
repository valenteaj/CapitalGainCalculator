using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;

namespace CapitalGainCalculator.CalculationEngine.Filters
{
    public class BedAndBreakfastFilter : ITransactionFilter
    {
        private static double CutOffDays = 30;
        public IEnumerable<Transaction> Filter(Transaction filterCandidate, IEnumerable<Transaction> toBeFiltered)
        {
            if (filterCandidate.TransactionType == TransactionType.Disposal)
            {
                var unsettled = toBeFiltered
                    .Where(t => 
                        t.TransactionType == TransactionType.Purchase &&
                        t.TransactionDate < filterCandidate.TransactionDate && 
                        t.TransactionDate > filterCandidate.TransactionDate.AddDays(CutOffDays * -1));
                
                if (unsettled.Any())
                {
                    var toReturn = toBeFiltered.Except(unsettled).ToList();

                    var unitCount = filterCandidate.NumberOfShares; // negative
                    var ordered = unsettled.OrderByDescending(t => t.TransactionDate);
                    foreach (var transaction in ordered)
                    {
                        unitCount += transaction.NumberOfShares;
                        if (unitCount > 0 && unitCount < transaction.NumberOfShares)
                        {
                            // Partial
                            toReturn.Add(new Transaction(
                                TransactionType.Purchase, 
                                transaction.Asset, 
                                transaction.TransactionDate, 
                                transaction.UnitPrice,
                                -unitCount, 
                                transaction.TransactionCosts
                            ));
                        }
                        else if (unitCount <= 0)
                        {
                            // Total
                            toReturn.Add(new Transaction(
                                TransactionType.Purchase, 
                                transaction.Asset, 
                                transaction.TransactionDate, 
                                transaction.UnitPrice, 
                                0, 
                                transaction.TransactionCosts
                            ));
                        }
                        else
                        {
                            // Overflow
                            toReturn.Add(transaction);
                        }
                    }
                    return toReturn.OrderBy(t => t.TransactionDate);
                }
            } 
            return toBeFiltered;
        }
    }
}