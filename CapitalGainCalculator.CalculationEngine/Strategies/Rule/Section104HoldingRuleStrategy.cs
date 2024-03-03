using CapitalGainCalculator.Common.Interfaces;
using CapitalGainCalculator.Common.Models;

namespace CapitalGainCalculator.CalculationEngine.Strategies.Rule
{
    public class Section104HoldingRuleStrategy : IRuleStrategy
    {
        public byte Priority => 100;

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
            Console.WriteLine($"{context.TransactionType} at {context.TransactionDate} executed {nameof(Section104HoldingRuleStrategy)}");
            return Filter(transactions, context);
        }

        public GainData Reduce(GainData gainData, IEnumerable<Transaction> matchedTransactions, Transaction context, decimal unitsSold)
        {
            var extracted = gainData.RemainingTransactions.Except(matchedTransactions);
            var pool = SquashTransactionsIntoPool(matchedTransactions);
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
            gainData.RemainingTransactions = remainingTransactions;
            gainData.Gains.Add(gain);
            return gainData;
        }

        private Transaction SquashTransactionsIntoPool(IEnumerable<Transaction> transactions)
        {
            var numberOfUnits = transactions.Sum(t => t.NumberOfShares);
            var costs = transactions.Sum(t => t.TransactionCosts);
            var averageUnitPrice = transactions.Sum(t => (t.UnitPrice * t.NumberOfShares) + t.TransactionCosts) / numberOfUnits;
            var lastTransaction = transactions.Last();
            return new Transaction
            (
                lastTransaction.TransactionType,
                lastTransaction.Asset,
                lastTransaction.TransactionDate,
                averageUnitPrice,
                numberOfUnits,
                0
            );
        }

        private IEnumerable<Transaction> Filter(IEnumerable<Transaction> transactions, Transaction context)
        {
            return transactions.Where(t => t.TransactionDate.Date <= context.TransactionDate.Date && 
                t.TransactionType == TransactionType.Purchase);
        }
    }
}