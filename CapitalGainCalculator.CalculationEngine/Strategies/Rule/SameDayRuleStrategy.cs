using CapitalGainCalculator.Common.Interfaces;
using CapitalGainCalculator.Common.Models;

namespace CapitalGainCalculator.CalculationEngine.Strategies.Rule
{
    public class SameDayRuleStrategy : IRuleStrategy
    {
        public byte Priority => 200;
        public string RuleName => "Same-Day Rule";

        public bool CanExecute(IEnumerable<Transaction> transactions, Transaction context)
        {
            if (context.TransactionType == TransactionType.Disposal)
            {
                return Filter(transactions, context).Any();
            }
            return false;
        }

        public IEnumerable<Transaction> Match(IEnumerable<Transaction> transactions, Transaction context)
        {
            Console.WriteLine($"{context.TransactionType} at {context.TransactionDate} executed {nameof(SameDayRuleStrategy)}");
            return Filter(transactions, context);
        }

        public ReportData Reduce(ReportData gainData, IEnumerable<Transaction> matchedTransactions, Transaction context, decimal unitsSold)
        {
            var extracted = gainData.UnmatchedTransactions.Except(matchedTransactions);
            var pool = matchedTransactions.First();
            var proportionOfSale = unitsSold / Math.Abs(context.NumberOfShares);

            var dispoalProceeds = unitsSold * context.UnitPrice - (context.TransactionCosts * proportionOfSale);
            var fractionOfSharesSold = unitsSold / pool.NumberOfShares;
            var allowableExpense = pool.NumberOfShares * pool.UnitPrice + pool.TransactionCosts;

            var gain = Math.Floor(dispoalProceeds) - Math.Ceiling(allowableExpense * fractionOfSharesSold);
            var remainingTransactions = new List<Transaction>(extracted);
            var remainingUnits = pool.NumberOfShares - unitsSold;
            if (remainingUnits > 0)
            {
                var replacementPool = pool with 
                {
                    NumberOfShares = remainingUnits
                };
                remainingTransactions.Add(replacementPool);
            }
            gainData.UnmatchedTransactions = remainingTransactions;
            gainData.Gains.Add(gain);
            return gainData;
        }

        private IEnumerable<Transaction> Filter(IEnumerable<Transaction> transactions, Transaction context)
        {
            return transactions.Where(t => t.TransactionDate.Date == context.TransactionDate.Date && 
                    t.TransactionType == TransactionType.Purchase);
        }
    }
}