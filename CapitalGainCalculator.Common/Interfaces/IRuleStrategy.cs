using CapitalGainCalculator.Common.Models;

namespace CapitalGainCalculator.Common.Interfaces
{
    public interface IRuleStrategy
    {
        public byte Priority {get;}
        public string RuleName {get;}
        public bool CanExecute(IEnumerable<Transaction> transactions, Transaction context);
        public IEnumerable<Transaction> Match(IEnumerable<Transaction> transactions, Transaction context);
        public ReportData Reduce(ReportData gainData, IEnumerable<Transaction> matchedTransactions, Transaction context, decimal unitsSold);
    }
}