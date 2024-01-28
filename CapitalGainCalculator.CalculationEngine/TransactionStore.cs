using CapitalGainCalculator.CalculationEngine.Interfaces;
using CapitalGainCalculator.CalculationEngine.Models;

namespace CapitalGainCalculator.CalculationEngine
{
    public class TransactionStore : IStore<Transaction>
    {
        private readonly ICollection<Transaction> _transactions = new List<Transaction>();
        public void Add(Transaction item) => _transactions.Add(item ?? throw new ArgumentException("No transaction provided", "item"));

        public IEnumerable<Transaction> Get()
        {
            return _transactions;
        }
    }
}