using CapitalGainCalculator.Common.Interfaces;
using CapitalGainCalculator.Common.Models;

namespace CapitalGainCalculator.CalculationEngine.Validators
{
    public class PortfolioValidator : IPortfolioValidator
    {
        public void Validate(IEnumerable<Transaction> transactions)
        {
            ValidateTransactionSequence(transactions.OrderBy(t => t.TransactionDate));
        }

        private void ValidateTransactionSequence(IOrderedEnumerable<Transaction> transactions)
        {
            decimal units = 0;
            foreach (var transaction in transactions)
            {
                units += transaction.NumberOfShares;
                if (units < 0)
                    throw new ArgumentException($"{transaction.TransactionType}: {transaction.TransactionDate} results in {Math.Abs(units)} more sold units than owned.", nameof(transactions));
            }
        }
    }
}