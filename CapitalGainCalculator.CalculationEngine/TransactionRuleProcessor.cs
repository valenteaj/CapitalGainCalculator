using CapitalGainCalculator.Common.Interfaces;
using CapitalGainCalculator.Common.Models;

namespace CapitalGainCalculator.CalculationEngine;
public class TransactionRuleProcessor : ITransactionProcessor
{
    private readonly IEnumerable<IRuleStrategy> _ruleStrategies;
    private readonly IEnumerable<IMutatorStrategy> _mutatorStrategies;
    private readonly IPortfolioValidator? _portfolioValidator;

    public TransactionRuleProcessor(
        IEnumerable<IRuleStrategy> ruleStrategies, 
        IEnumerable<IMutatorStrategy> mutatorStrategies,
        IPortfolioValidator? portfolioValidator)
    {
        _mutatorStrategies = mutatorStrategies;
        _portfolioValidator = portfolioValidator;
        _ruleStrategies = ruleStrategies;
    }

    public T Process<T>(IEnumerable<Transaction> transactions, IReporter<T> reporter)
    {
        _portfolioValidator?.Validate(transactions);
        IEnumerable<Transaction> preMatched = new List<Transaction>(transactions);
        foreach (var mutatorStrategy in _mutatorStrategies)
        {
            preMatched = mutatorStrategy.Execute(preMatched);
        }

        var gainData = new ReportData(preMatched);
        foreach (var transaction in preMatched.OrderBy(t => t.TransactionDate))
        {
            var strategies = _ruleStrategies
                .Where(s => s.CanExecute(gainData.UnmatchedTransactions, transaction))
                .OrderByDescending(s => s.Priority);

            var units = Math.Abs(transaction.NumberOfShares);
            foreach (var strategy in strategies)
            {
                var matches = strategy.Match(gainData.UnmatchedTransactions, transaction);
                var matchUnits = Math.Min(matches.Sum(t => t.NumberOfShares), units);
                var reduced = strategy.Reduce(gainData, matches, transaction, matchUnits);
                units -= matchUnits;
                if (units == 0)
                {
                    break;
                }
            }
        }
        return reporter.GenerateReport(gainData);
    }
}
