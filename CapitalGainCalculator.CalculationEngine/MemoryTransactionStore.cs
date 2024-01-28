using CapitalGainCalculator.CalculationEngine.Models;

namespace CapitalGainCalculator.CalculationEngine
{
    public class MemoryTransactionStore : ITransactionStore
    {
        private readonly ICollection<Transaction> _transactions = new List<Transaction>();

        public IEnumerable<Transaction> Get()
        {
            return _transactions;
        }

        public void Add(Transaction transaction)
        {
            _transactions.Add(transaction ?? throw new ArgumentException("No transaction provided"));
        }
    }
}