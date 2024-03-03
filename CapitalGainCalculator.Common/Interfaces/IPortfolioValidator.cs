using CapitalGainCalculator.Common.Models;

namespace CapitalGainCalculator.Common.Interfaces
{
    public interface IPortfolioValidator
    {
        public void Validate(IEnumerable<Transaction> transactions);
    }
}