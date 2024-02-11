using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;

namespace CapitalGainCalculator.CalculationEngine
{
    public class TransactionStore : IStore<Transaction>
    {
        private readonly ICollection<Transaction> _transactions = new List<Transaction>();
        public void Add(Transaction item) => _transactions.Add(item ?? throw new ArgumentException("No transaction provided", "item"));

        public IEnumerable<Transaction> Filter(IFilter<Transaction> filter, Transaction filterCandidate)
        {
            return filter.Filter(filterCandidate, _transactions);
        }

        public IEnumerable<Transaction> Filter(IEnumerable<IFilter<Transaction>> filters, Transaction filterCandidate)
        {
            IEnumerable<Transaction> transactions = _transactions;
            foreach (var filter in filters)
            {
                transactions = Filter(filter, filterCandidate);
            }
            return transactions;
        }

        public IEnumerable<Transaction> Get()
        {
            return _transactions;
        }
    }
}