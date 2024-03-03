using CapitalGainCalculator.Common.Interfaces;
using CapitalGainCalculator.Common.Models;

namespace CapitalGainCalculator.ReportGenerator.Reporters
{
    public class DecimalGainReporter : IReporter<decimal>
    {
        public decimal GenerateReport(ReportData gainData)
        {
            return gainData.Gains.Sum();
        }
    }
}