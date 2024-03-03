namespace CapitalGainCalculator.Common.Models
{
    public class GainData
    {
        public GainData(IEnumerable<Transaction> remainingTransactions)
        {
            RemainingTransactions = remainingTransactions;
            Gains = new List<decimal>();
        }
        public IEnumerable<Transaction> RemainingTransactions {get;set;}
        public List<decimal> Gains {get;set;}
    }
}