using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;

namespace CapitalGainCalculator.CalculationEngine.Strategies
{
    public class TransactionStrategyCoordinator
    {
        private ITransactionStrategy? _activeStrategy;
        private readonly Dictionary<TransactionType, ITransactionStrategy> _strategies = new();

        public TransactionStrategyCoordinator(IEnumerable<ITransactionStrategy> strategies)
        {
            foreach (var strategy in strategies)
            {
                _strategies.Add(strategy.TransactionType, strategy);
            }
        }

        public void SetStrategy(TransactionType transactionType)
        {
            if (!_strategies.ContainsKey(transactionType))
            {
                throw new InvalidOperationException($"No stategy defined to handle {transactionType} transactions");
            }
            _activeStrategy = _strategies[transactionType];
        }

        public CumulativeGainData ExecuteAggregate(Transaction transaction, CumulativeGainData accumulator)
        {
            return _activeStrategy?.Aggregate(transaction, accumulator) ?? throw new InvalidOperationException("No transaction strategy set");
        }
    }
}