using CapitalGainCalculator.Common.Interfaces;
using CapitalGainCalculator.Common.Models;

namespace CapitalGainCalculator.CalculationEngine.Strategies.Rule
{
    public class ThirtyDayRuleStrategy : IRuleStrategy
    {
        public byte Priority => 150;
        public string RuleName => "30-Day Rule";

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
            Console.WriteLine($"{context.TransactionType} at {context.TransactionDate} executed {nameof(ThirtyDayRuleStrategy)}");
            decimal cumulativeUnits = 0;
            var result = new List<Transaction>();
            var filteredTransactions = new Queue<Transaction>(Filter(transactions, context));
            var normalisedContextUnitCount = Math.Abs(context.NumberOfShares);

            while (cumulativeUnits < normalisedContextUnitCount && filteredTransactions.TryDequeue(out var transaction))
            {
                cumulativeUnits += transaction.NumberOfShares;
                result.Add(transaction);
            }
            return result;
        }

        public ReportData Reduce(ReportData gainData, IEnumerable<Transaction> matchedTransactions, Transaction context, decimal unitsSold)
        {
            var extracted = gainData.UnmatchedTransactions.Except(matchedTransactions);
            var eclipsedPurchases = matchedTransactions.Except(new[] {matchedTransactions.Last()});
            var lastPurchase = matchedTransactions.Last();

            var proportionOfSale = unitsSold / Math.Abs(context.NumberOfShares);
            var dispoalProceeds = unitsSold * context.UnitPrice - (context.TransactionCosts * proportionOfSale);
            var remainingUnits = unitsSold;

            decimal eclipsedAllowableExpense = 0;
            foreach (var matched in eclipsedPurchases)
            {
                eclipsedAllowableExpense += matched.NumberOfShares * matched.UnitPrice + matched.TransactionCosts;
                remainingUnits -= matched.NumberOfShares;
            }

            var partialAllowableExpense = lastPurchase.NumberOfShares * lastPurchase.UnitPrice + lastPurchase.TransactionCosts;

            var fractionOfSharesSold = remainingUnits / lastPurchase.NumberOfShares;
            var gain = Math.Floor(dispoalProceeds) - Math.Ceiling(eclipsedAllowableExpense + (partialAllowableExpense * fractionOfSharesSold));

            var remainingTransactions = new List<Transaction>(extracted);
            if (lastPurchase.NumberOfShares - remainingUnits > 0)
            {
                var replacementPool = lastPurchase with 
                {
                    NumberOfShares = lastPurchase.NumberOfShares - remainingUnits,
                    TransactionCosts = lastPurchase.TransactionCosts * (1-fractionOfSharesSold)
                };
                remainingTransactions.Add(replacementPool);
            }
            gainData.UnmatchedTransactions = remainingTransactions;
            gainData.Gains.Add(gain);
            return gainData;
        }
        
        private Transaction SquashTransactions(IEnumerable<Transaction> transactions)
        {
            var numberOfUnits = transactions.Sum(t => t.NumberOfShares);
            var costs = transactions.Sum(t => t.TransactionCosts);
            var averageUnitPrice = transactions.Sum(t => (t.UnitPrice * t.NumberOfShares) + t.TransactionCosts) / numberOfUnits;
            var lastTransaction = transactions.Last();
            return new Transaction(
                lastTransaction.TransactionType,
                lastTransaction.Asset,
                lastTransaction.TransactionDate,
                numberOfUnits,
                averageUnitPrice,
                0);
        }

        private IEnumerable<Transaction> Filter(IEnumerable<Transaction> transactions, Transaction context)
        {
            return transactions.Where(t => t.TransactionDate.Date > context.TransactionDate.Date && 
                    t.TransactionDate.Date <= context.TransactionDate.Date.AddDays(30) &&
                    t.TransactionType == TransactionType.Purchase);
        }
    }
}