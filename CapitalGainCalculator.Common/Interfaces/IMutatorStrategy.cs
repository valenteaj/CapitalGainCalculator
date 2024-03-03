using CapitalGainCalculator.Common.Models;

namespace CapitalGainCalculator.Common.Interfaces
{
    public interface IMutatorStrategy
    {
        public bool CanExecute(IEnumerable<Transaction> transactions);
        public IEnumerable<Transaction> Execute(IEnumerable<Transaction> transactions);
    }
}