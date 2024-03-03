using CapitalGainCalculator.Common.Interfaces;
using CapitalGainCalculator.Common.Models;

namespace CapitalGainCalculator.CalculationEngine.Strategies.Mutator
{
    public class SameDayDisposalMutatorStrategy : IMutatorStrategy
    {
        public bool CanExecute(IEnumerable<Transaction> transactions) => true;

        public IEnumerable<Transaction> Execute(IEnumerable<Transaction> transactions)
        {
            var result = new List<Transaction>(transactions);
            var sameDayPurchases = transactions
                .Where(t => t.TransactionType == TransactionType.Disposal)
                .GroupBy(t => t.TransactionDate.Date)
                .Where(grp => grp.Count() > 1);

            result = result.Except(sameDayPurchases.SelectMany(t => t)).ToList();
            var aggregatedTransactions = sameDayPurchases
                .Select(grp => new Transaction(
                    TransactionType.Disposal, 
                    grp.First().Asset, 
                    grp.First().TransactionDate, 
                    grp.Sum(t => t.UnitPrice), 
                    grp.Sum(t => t.NumberOfShares), 
                    grp.Sum(t => t.TransactionCosts))
                );
            result.AddRange(aggregatedTransactions);

            return result.OrderBy(t => t.TransactionDate);
        }
    }
}