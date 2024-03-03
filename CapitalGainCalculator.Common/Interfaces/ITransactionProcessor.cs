using CapitalGainCalculator.Common.Models;

namespace CapitalGainCalculator.Common.Interfaces
{
    public interface ITransactionProcessor
    {
        public T Process<T>(IEnumerable<Transaction> transactions, IReporter<T> reporter);
    }
}