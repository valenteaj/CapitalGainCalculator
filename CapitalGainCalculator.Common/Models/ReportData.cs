namespace CapitalGainCalculator.Common.Models
{
    public class ReportData
    {
        public ReportData(IEnumerable<Transaction> remainingTransactions)
        {
            UnmatchedTransactions = remainingTransactions;
            Gains = new List<decimal>();
        }
        public IEnumerable<Transaction> UnmatchedTransactions {get;set;}
        public List<decimal> Gains {get;set;}
    }
}